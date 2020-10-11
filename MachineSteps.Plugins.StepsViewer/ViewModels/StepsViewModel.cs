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

namespace MachineSteps.Plugins.StepsViewer.ViewModels
{
    public class StepsViewModel : ViewModelBase
    {
        private bool _autoStepOver;

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
        }

        private void OnMaterialRemovalMessage(MaterialRemovalMessage msg) => LinearLinkMovementManager.EnableMaterialRemoval = msg.Active;

        private void OnAutoStepOverChangedMessage(AutoStepOverChangedMessage msg) => _autoStepOver = msg.Value;

        private void OnStepCompleteMessage(StepCompleteMessage msg)
        {
            if(_autoStepOver)
            {
                Task.Run(async () =>
                {
                    await Task.Delay(50);

                    StepViewModel newSelection = GetNextStep();

                    if (newSelection != null) DispatcherHelper.CheckBeginInvokeOnUI(() => Selected = newSelection);
                });
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
