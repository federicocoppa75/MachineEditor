using GalaSoft.MvvmLight;
using MachineSteps.Models.Steps;
using MachineSteps.Plugins.StepsViewer.Enums;
using MachineSteps.Plugins.StepsViewer.Extensions;
using MachineSteps.Plugins.StepsViewer.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineSteps.Plugins.StepsViewer.ViewModels
{
    public class StepViewModel : ViewModelBase
    {
        public int Id { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public int Index { get; private set; }
        public int Channel { get; private set; }

        private bool _durationIsValid;
        private double _duration;
        public double Duration
        {
            get
            {
                if (!_durationIsValid) InitDuration();
                return _duration;
            }
        }

        public double EvolutionTime { get; set; }

        private StepState _state;
        public StepState State
        {
            get => _state;
            set => Set(ref _state, value, nameof(State)); 
        }

        public List<ActionViewModel> FarwardActions { get; private set; } = new List<ActionViewModel>();
        public List<ActionViewModel> BackActions { get; private set; } = new List<ActionViewModel>();

        public StepViewModel(MachineStep step, int index = 0)
        {
            Id = step.Id;
            Name = step.Name;
            Description = step.Description;
            Index = index;
            Channel = step.Channel;

            step.Actions.ForEach((a) =>
            {
                FarwardActions.Add(new ActionViewModel(a));
                BackActions.Add(new ActionViewModel(a.CreateBackStepAction()));
            });

            MessengerInstance.Register<ActionCompleteMessage>(this, OnActionCompleteMessage);
        }

        public StepViewModel(int id, string name, string description, int index = 0)
        {
            Id = id;
            Name = name;
            Description = description;
            Index = index;
        }

        public void ExecuteFarward()
        {
            State = StepState.Executing;
            FarwardActions.ForEach(a => a.Execute(true));
        }

        public void ExecuteBack()
        {
            BackActions.ForEach(a => a.Execute());
            State = StepState.ToStart;
            FarwardActions.ForEach(a => a.IsCompleted = false);
        }

        public void UpdateLazys() => BackActions.ForEach(a => a.UpdateLazy());

        private void InitDuration()
        {
            if(FarwardActions.Count > 0) _duration = FarwardActions.Select(a => a.Duration).Max();
            _durationIsValid = true;
        }

        private void CheckCompleted()
        {
            if (FarwardActions.All(a => a.IsCompleted))
            {
                State = StepState.Finished;
                MessengerInstance.Send(new StepCompleteMessage() { Id = Id, Index = Index, Channel = Channel });
            }
        }

        private void OnActionCompleteMessage(ActionCompleteMessage msg)
        {
            if (State == StepState.Executing) CheckCompleted(); 
        }
    }
}
