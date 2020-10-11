using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using HelixToolkit.Wpf;
using MachineSteps.Models;
using MachineSteps.Plugins.StepsViewer.Messages;
using MachineViewer.Plugins.Common.Messages.Links;

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
                    var doc = IsoParser.IsoParser.Parse(dlg.FileName, true);

                    if (doc != null)
                    {
                        ShowMacineStepsDocument(doc);
                    }
                }
            }            
        }

        private void ShowMacineStepsDocument(MachineStepsDocument doc)
        {
            Messenger.Default.Send(new LoadStepsMessage() { Steps = doc.Steps });
            if (doc.Steps.Count > 0) IsStepTimeVisible = true;
        }

        private void UnloadStepsCommandImplementation()
        {
            var dynamicTransition = DynamicTransition;
            var autoStepOver = AutoStepOver;
            DynamicTransition = false;
            AutoStepOver = false;

            Messenger.Default.Send(new UnloadStepsMessage());
            IsStepTimeVisible = false;

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
    }
}
