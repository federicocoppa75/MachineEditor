using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using MachineSteps.Models.Actions;
using MachineSteps.Models.Steps;
using MachineSteps.Plugins.StepsViewer.Messages;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MachineSteps.Plugins.StepsViewer.Extensions;
using MachineSteps.Plugins.StepsViewer.Models;
using GalaSoft.MvvmLight.Threading;
using MachineViewer.Utilities;
using MachineViewer.Plugins.Common.Messages.Generic;

namespace MachineSteps.Plugins.StepsViewer.ViewModels
{
    public class StepsViewModel : ViewModelBase
    {
        private bool _autoStepOver;
        private bool _multiChannel;
        private bool[] _channelBusy = new bool[2];
        private int[] _channelFreeBackNotifyId = new int[2];

        public ObservableCollection<StepViewModel> Steps { get; private set; } = new ObservableCollection<StepViewModel>();

        private StepViewModel _selected;

        public StepViewModel Selected
        {
            get { return _selected; }
            set
            {
                var lastSelected = _selected;

                if(Set(ref _selected, value, nameof(Selected)))
                {
                    ManageSelectionChanged(_selected, lastSelected);
                }
            }
        }

        public StepsViewModel() : base()
        {
            MessengerInstance.Register<LoadStepsMessage>(this, OnLoadStepsMessage);
            MessengerInstance.Register<UnloadStepsMessage>(this, OnUnloadStepsMessage);
            MessengerInstance.Register<StepCompleteMessage>(this, OnStepCompleteMessage);
            MessengerInstance.Register<AutoStepOverChangedMessage>(this, OnAutoStepOverChangedMessage);
            MessengerInstance.Register<MaterialRemovalMessage>(this, OnMaterialRemovalMessage);
            MessengerInstance.Register<MultiChannelMessage>(this, OnMultiChannelMessage);
            MessengerInstance.Register<WaitForChannelFreeMessage>(this, OnWaitForChannelFreeMessage);
        }

        private void OnWaitForChannelFreeMessage(WaitForChannelFreeMessage msg)
        {
            if(!_channelBusy[msg.Channel])
            {
                MessengerInstance.Send(new BackNotificationMessage() { DestinationId = msg.BackNotifyId });
            }
            else
            {
                _channelFreeBackNotifyId[msg.Channel] = msg.BackNotifyId;
            }
        }

        private void OnMultiChannelMessage(MultiChannelMessage msg) => _multiChannel = msg.Value;

        private void OnMaterialRemovalMessage(MaterialRemovalMessage msg) => LinearLinkMovementManager.EnableMaterialRemoval = msg.Active;

        private void OnAutoStepOverChangedMessage(AutoStepOverChangedMessage msg) => _autoStepOver = msg.Value;

        private void OnStepCompleteMessage(StepCompleteMessage msg)
        {
            if(_autoStepOver)
            {
                if(_multiChannel)
                {
                    Task.Run(async () =>
                    {
                        await Task.Delay(50);

                        if (msg.Channel > 0)
                        {
                            var step = GetNextStep(msg.Channel, msg.Index);

                            if(step != null)
                            {
                                _channelBusy[1] = true;
                                step.UpdateLazys();
                                step.ExecuteFarward();
                            }
                            else
                            {
                                _channelBusy[1] = false;

                                if(_channelFreeBackNotifyId[1] > 0)
                                {
                                    var id = _channelFreeBackNotifyId[1];

                                    _channelFreeBackNotifyId[1] = 0;
                                    MessengerInstance.Send(new BackNotificationMessage() { DestinationId = id });
                                }
                            }
                        }
                        else
                        {
                            _channelBusy[0] = false;
                        }

                        if (!_channelBusy[0])
                        {
                            StepViewModel newSelection = GetNextStep(0, msg.Index);

                            if (newSelection != null)
                            {
                                _channelBusy[0] = true;
                                DispatcherHelper.CheckBeginInvokeOnUI(() =>
                                {
                                    _selected = newSelection;
                                    RaisePropertyChanged(nameof(Selected));
                                    _selected.UpdateLazys();
                                    _selected.ExecuteFarward();
                                });
                            }
                        }
                    });
                }
                else
                {
                    Task.Run(async () =>
                    {
                        await Task.Delay(50);

                        StepViewModel newSelection = GetNextStep();

                        if (newSelection != null) DispatcherHelper.CheckBeginInvokeOnUI(() => Selected = newSelection);
                    });
                }

            }
        }

        private StepViewModel GetNextStep()
        {
            StepViewModel newSelection = null;

            if (_selected == null)
            {
                newSelection = Steps[0];
            }
            else
            {
                int index = Steps.IndexOf(Selected) + 1;

                if (index < Steps.Count())
                {
                    newSelection = Steps[index];
                }
            }

            return newSelection;
        }

        private StepViewModel GetNextStep(int channel, int fromIndex)
        {
            StepViewModel nextStep = null;

            for (int i = fromIndex + 1; i < Steps.Count; i++)
            {
                if(Steps[i].Channel == channel)
                {
                    nextStep = Steps[i];
                    break;
                }
                else if(IsChannelWaiter(Steps[i], channel))
                {
                    break;
                }
            }

            return nextStep;
        }

        private bool IsChannelWaiter(StepViewModel step, int channel)
        {
            bool result = false;

            if((step.FarwardActions.Count == 1) && 
                (step.FarwardActions[0].Action is ChannelWaiterAction action) && 
                (action.ChannelToWait == channel))
            {
                result = true;
            }

            return result;
        }

        private void OnUnloadStepsMessage(UnloadStepsMessage msg)
        {
            if(Steps.Count > 0)
            {
                Selected = Steps[0];
                Steps.Clear();
                Selected = null;
            }
        }

        private void OnLoadStepsMessage(LoadStepsMessage msg)
        {
            if((msg != null) && (msg.Steps != null) && (msg.Steps.Count > 0))
            {
                Steps.Add(new StepViewModel(-1, "Start", "Condizione iniziale"));

                for (int i = 0; i < msg.Steps.Count; i++)
                {
                    Steps.Add(new StepViewModel(msg.Steps[i], i + 1));
                }

                UpdateEvolutionTime();
            }
        }

        private void ManageSelectionChanged(StepViewModel selected, StepViewModel lastSelected)
        {
            if(lastSelected == null)
            {
                ManageFarwardSelectionChanged(selected, Steps[0]);
            }
            else if(selected == null)
            {
                // per il momento non faccio nulla
            }
            else if(selected.Index > lastSelected.Index)
            {
                ManageFarwardSelectionChanged(selected, lastSelected);
            }
            else if (selected.Index < lastSelected.Index)
            {
                ManageBackSelectionChanged(selected, lastSelected);
            }

            MessengerInstance.Send(new UpdateStepTimeMessage() { Time = (Selected != null) ? Selected.EvolutionTime : 0.0 });
        }

        private void ManageBackSelectionChanged(StepViewModel selected, StepViewModel lastSelected)
        {
            MessengerInstance.Send(new SuspendPlaybackSettingsMessage());

            for (int i = lastSelected.Index; i > selected.Index; i--)
            {
                Steps[i].ExecuteBack();
            }

            MessengerInstance.Send(new ResumePlaybackSettingsMessage());
        }

        private void ManageFarwardSelectionChanged(StepViewModel selected, StepViewModel lastSelected)
        {
            for (int i = lastSelected.Index+1; i <= selected.Index; i++)
            {
                var svm = Steps[i];

                svm.UpdateLazys();
                svm.ExecuteFarward();
            }
        }

        private void UpdateEvolutionTime()
        {
            double time = 0.0;

            for (int i = 0; i < Steps.Count; i++)
            {
                time += Steps[i].Duration;
                Steps[i].EvolutionTime = time;
            }
        }

        private bool IsSelectedFirst() => (Selected != null) ? Steps.IndexOf(Selected) == 0 : false;
    }
}
