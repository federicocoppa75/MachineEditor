using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using HelixToolkit.Wpf;
using MachineSteps.Models;
using MachineSteps.Models.Actions;
using MachineSteps.Plugins.StepsViewer.Messages;
using MachineViewer.Extensions;
using MachineViewer.Plugins.Common.Messages.Generic;
using MachineViewer.Plugins.Common.Messages.Inverters;
using MachineViewer.Plugins.Common.Messages.Links;
using MachineViewer.Plugins.Links.SimpleManipolator.Messages;
using MachineViewModels.ViewModels.Links;

namespace MachineSteps.Plugins.StepsViewer.ViewModels
{
    public class MainViewModel : MachineViewer.ViewModels.MainViewModel
    {
        private bool _suspendedAutoStepOverValue;
        private bool _suspendedDynamicTransition;

        private LightSetup _light;
        public LightSetup Light
        {
            get => _light;
            set
            {
                if ((_light == null) || (_light.GetType() != value.GetType()))
                {
                    if (_light != null) Models.Remove(_light);
                    _light = value;
                    Models.Add(_light);
                    RaisePropertyChanged(nameof(IsSunLight));
                    RaisePropertyChanged(nameof(IsDefaultLights));
                    RaisePropertyChanged(nameof(IsSpotHeadLight));
                }
            }
        }

        public bool IsSunLight
        {
            get => Light is SunLight;
            set
            {
                if (value) Light = new SunLight();
            }
        }

        public bool IsDefaultLights
        {
            get => Light is DefaultLights;
            set
            {
                if (value) Light = new DefaultLights();
            }
        }

        public bool IsSpotHeadLight
        {
            get => Light is SpotHeadLight;
            set
            {
                if (value) Light = new SpotHeadLight();
            }
        }

        private TimeSpan _stepTime = new TimeSpan();
        public TimeSpan StepTime
        {
            get => _stepTime;
            set => Set(ref _stepTime, value, nameof(StepTime));
        }

        private bool _isStepTimeVisible;
        public bool IsStepTimeVisible
        {
            get => _isStepTimeVisible;
            set => Set(ref _isStepTimeVisible, value, nameof(IsStepTimeVisible));
        }

        private bool _dynamicTransition;
        public bool DynamicTransition
        {
            get => _dynamicTransition; 
            set
            {
                if(Set(ref _dynamicTransition, value, nameof(DynamicTransition)))
                {
                    NotifyDynamicTransitionChanged();
                    if (!_dynamicTransition) AutoStepOver = false;
                }
            }
        }

        private bool _autoStepOver;
        public bool AutoStepOver
        {
            get => _autoStepOver;
            set
            {
                if(Set(ref _autoStepOver, value, nameof(AutoStepOver)))
                {
                    if(_autoStepOver) DynamicTransition = true;
                    MessengerInstance.Send(new AutoStepOverChangedMessage() { Value = _autoStepOver });
                }
            }
        }

        private bool _materialRemoval;
        public bool MaterialRemoval
        {
            get => _materialRemoval; 
            set
            {
                if(Set(ref _materialRemoval, value, nameof(MaterialRemoval)))
                {
                    MessengerInstance.Send(new MaterialRemovalMessage() { Active = _materialRemoval });
                }
            }
        }

        private bool _multiChannel;
        public bool MultiChannel
        {
            get => _multiChannel;
            set 
            { 
                if(Set(ref _multiChannel, value, nameof(MultiChannel)))
                {
                    MessengerInstance.Send(new MultiChannelMessage() { Value = _multiChannel });
                }
            }
        }


        //private string _inverterState;
        //public string InverterState
        //{
        //    get => _inverterState; 
        //    set => Set(ref _inverterState, value, nameof(InverterState));
        //}

        private string _inverterName;
        public string InverterName
        {
            get => _inverterName; 
            set => Set(ref _inverterName, value, nameof(InverterName)); 
        }

        private int _inverterValue;
        public int InverterValue
        {
            get => _inverterValue;
            set => Set(ref _inverterValue, value, nameof(InverterValue)); 
        }


        private bool _isInverterStateVisible;
        public bool IsInverterStateVisible
        {
            get => _isInverterStateVisible;
            set => Set(ref _isInverterStateVisible, value, nameof(IsInverterStateVisible));
        }

        private bool _isAxesStateVisible;

        public bool IsAxesStateVisible
        {
            get => _isAxesStateVisible; 
            set => Set(ref _isAxesStateVisible, value, nameof(IsAxesStateVisible));
        }


        public ObservableCollection<LinearPositionViewModel> LinearPositionLinks { get; set; } = new ObservableCollection<LinearPositionViewModel>();

        private ICommand _loadStepsCommand;
        public ICommand LoadStepsCommand { get { return _loadStepsCommand ?? (_loadStepsCommand = new RelayCommand(() => LoadStepsCommandImplementation())); } }

        private ICommand _unloadStepsCommand;
        public ICommand UnloadStepsCommand { get { return _unloadStepsCommand ?? (_unloadStepsCommand = new RelayCommand(() => UnloadStepsCommandImplementation())); } }

        public MainViewModel(HelixToolkit.Wpf.IHelixViewport3D viewport) : base(viewport, false)
        {
            Light = new SpotHeadLight();

            MessengerInstance.Register<UpdateStepTimeMessage>(this, OnUpdateStepTimeMessage);
            MessengerInstance.Register<SuspendPlaybackSettingsMessage>(this, OnSuspendPlaybackSettingsMessage);
            MessengerInstance.Register<ResumePlaybackSettingsMessage>(this, OnResumePlaybackSettingsMessage);
            MessengerInstance.Register<TurnOffInverterMessage>(this, OnTurnOffInverterMessage);
            MessengerInstance.Register<TurnOnInverterMessage>(this, OnTurnOnInverterMessage);
            MessengerInstance.Register<UpdateLinkViewModelsListMessage>(this, OnUpdateLinkViewModelsListMessage);
            MessengerInstance.Register<UpdateInverterMessage>(this, OnUpdateInverterMessage);
        }

        private void LoadStepsCommandImplementation()
        {
            var dlg = new Microsoft.Win32.OpenFileDialog() { DefaultExt = "msteps", AddExtension = true, Filter = "Machine steps |*.msteps|Cnc iso file |*.iso" };
            var b = dlg.ShowDialog();

            if (b.HasValue && b.Value)
            {
                var extension = System.IO.Path.GetExtension(dlg.FileName).Replace(".", "");

                if (string.Compare(extension, "msteps", true) == 0)
                {
                    var serializer = new System.Xml.Serialization.XmlSerializer(typeof(MachineStepsDocument));

                    using (var reader = new System.IO.StreamReader(dlg.FileName))
                    {
                        var doc = (MachineStepsDocument)serializer.Deserialize(reader);

                        if (doc != null)
                        {
                            ShowMacineStepsDocument(doc);
                        }
                    }
                }
                else if(string.Compare(extension, "iso", true) == 0)
                {
                    var doc = IsoParser.IsoParser.Parse(dlg.FileName, true, GetLinkLimits, GetLinearLinksCount);

                    if (doc != null)
                    {
                        ShowMacineStepsDocument(doc);
                    }
                }
            }            
        }

        private Tuple<double, double> GetLinkLimits(int linkId)
        {
            double min = 0.0, max = 0.0;

            MessengerInstance.Send(new ReadLinkLimitsMessage()
            {
                LinkId = linkId,
                SetLimits = (a, b) =>
                {
                    min = a;
                    max = b;
                }
            });

            return new Tuple<double, double>(min, max);
        }

        private void ShowMacineStepsDocument(MachineStepsDocument doc)
        {
            Messenger.Default.Send(new LoadStepsMessage() { Steps = doc.Steps });
            if (doc.Steps.Count > 0)
            {
                IsStepTimeVisible = true;
                IsAxesStateVisible = true;
            }
        }

        private void UnloadStepsCommandImplementation()
        {
            var dynamicTransition = DynamicTransition;
            var autoStepOver = AutoStepOver;
            DynamicTransition = false;
            AutoStepOver = false;

            Messenger.Default.Send(new UnloadStepsMessage());
            IsStepTimeVisible = false;
            IsAxesStateVisible = false;

            DynamicTransition = dynamicTransition;
            AutoStepOver = autoStepOver;
        }

        private void OnUpdateStepTimeMessage(UpdateStepTimeMessage msg) => StepTime = TimeSpan.FromSeconds(msg.Time);

        private void NotifyDynamicTransitionChanged()
        {
            MessengerInstance.Send(new DynamicTransitionChangedMessage() { Value = _dynamicTransition });
            MessengerInstance.Send(new EnableGradualTransitionMessage() { Value = _dynamicTransition });
            MessengerInstance.Send(new EnablePneumaticTransactionMessage() { Value = _dynamicTransition });
        }

        private void OnSuspendPlaybackSettingsMessage(SuspendPlaybackSettingsMessage obj)
        {
            _suspendedAutoStepOverValue = AutoStepOver;
            _suspendedDynamicTransition = DynamicTransition;
            AutoStepOver = false;
            DynamicTransition = false;
        }

        private void OnResumePlaybackSettingsMessage(ResumePlaybackSettingsMessage obj)
        {
            DynamicTransition = _suspendedDynamicTransition;
            AutoStepOver = _suspendedAutoStepOverValue;
        }

        private void OnTurnOffInverterMessage(TurnOffInverterMessage obj)
        {
            InverterName = string.Empty;
            IsInverterStateVisible = false;
            NotifyBack(obj.BackNotifyId, obj.Duration);
        }

        private void OnTurnOnInverterMessage(TurnOnInverterMessage msg)
        {
            if (!IsInverterStateVisible) IsInverterStateVisible = true;

            int invId = 0;

            if(msg.Order == 0)
            {
                switch (msg.Head)
                {
                    case 1:
                    case 2:
                    case 12:
                    case 21:
                        invId = 2;
                        break;
                    default:
                        throw new ArgumentException("Invalid head id!!!");
                }
            }
            else if(msg.Head == 1)
            {
                switch(msg.Order)
                {
                    case 1:
                        invId = 1;
                        break;
                    case 2:
                        invId = 3;
                        break;
                    default:
                        throw new ArgumentException("Invalid order id!!!");
                }
            }
            else
            {
                throw new InvalidOperationException("Invalid inverter id!!!");
            }

            InverterName = $"S{invId}";
            InverterValue = msg.RotationSpeed;
            NotifyBack(msg.BackNotifyId, msg.Duration);
        }

        private void OnUpdateInverterMessage(UpdateInverterMessage msg)
        {
            InverterValue = msg.RotationSpeed;
            NotifyBack(msg.BackNotifyId, msg.Duration);
        }

        private void NotifyBack(int bakNotifyId, double duration)
        {
            if (bakNotifyId > 0)
            {
                if (DynamicTransition)
                {
                    Task.Delay(TimeSpan.FromSeconds(duration))
                        .ContinueWith((t) =>
                        {
                            Messenger.Default.Send(new BackNotificationMessage() { DestinationId = bakNotifyId });
                        });
                }
                else
                {
                    Messenger.Default.Send(new BackNotificationMessage() { DestinationId = bakNotifyId });
                }
            }
        }

        private void OnUpdateLinkViewModelsListMessage(UpdateLinkViewModelsListMessage msg)
        {
            var list = msg.LinkViewModels.Where((lnk) => lnk is LinearPositionViewModel)
                                         .Cast<LinearPositionViewModel>()
                                         .OrderBy((lpvm) => AxOrder(lpvm.Id))       // sarebbe da fare nella vista ma non avevo voglia!
                                         .ToList();

            LinearPositionLinks.Clear();

            for (int i = 0; i < list.Count; i++) LinearPositionLinks.Add(list[i]);
        }

        private int AxOrder(int axId)
        {
            int result = -1;

            switch (axId)
            {
                case 1: result = 1; break;
                case 2: result = 2; break;
                case 101: result = 3; break;
                case 201: result = 4; break;
                case 102: result = 5; break;
                case 202: result = 6; break;
                case 112: result = 7; break;
                case 212: result = 8; break;
                default: break;
            }

            return result;
        }
    }
}
