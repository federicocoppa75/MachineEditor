using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using MachineSteps.Editor.Messages;
using MachineSteps.Models.Actions;
using MachineSteps.Models.Enums;
using MachineSteps.Models.Steps;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MachineSteps.Editor.ViewModels
{
    class MachineStepViewModel : ViewModelBase
    {
        private static int _index = 1;

        private MachineStep _machineStep;
        public MachineStep MachineStep
        {
            get { return _machineStep; }
            set { Set(ref _machineStep, value, nameof(MachineStep)); }
        }

        public ObservableCollection<BaseAction> Actions { get; set; } = new ObservableCollection<BaseAction>();

        private BaseAction _selected;

        public BaseAction Selected
        {
            get { return _selected; }
            set
            {
                if(Set(ref _selected, value, nameof(Selected)))
                {
                    MessengerInstance.Send(new ActionSelectionChangedMessage() { Action = _selected });
                }
            }
        }

        public Array ActionTypes => Enum.GetValues(typeof(ActionType));

        private ActionType _actionType;

        public ActionType ActionType
        {
            get { return _actionType; }
            set { Set(ref _actionType, value, nameof(ActionType)); }
        }

        private ICommand _addActionCommand;
        public ICommand AddActionCommand => _addActionCommand ?? (_addActionCommand = new RelayCommand(AddActionCommandImpl, () => MachineStep != null));

        private ICommand _delActionCommand;
        public ICommand DelActionCommand => _delActionCommand ?? (_delActionCommand = new RelayCommand(DelActionCommandImpl, () => Selected != null));

        public MachineStepViewModel()
        {
            MessengerInstance.Register<MachineStepSelectionChangedMessage>(this, OnMachineStepSelectionChangedMessage);
        }

        private void OnMachineStepSelectionChangedMessage(MachineStepSelectionChangedMessage msg)
        {
            MachineStep = msg.MachineStep;

            Actions.Clear();

            if(MachineStep != null)
            {
                foreach (var item in MachineStep.Actions)
                {
                    Actions.Add(item);
                }
            }
        }

        private void AddActionCommandImpl()
        {
            BaseAction action = null;

            switch (ActionType)
            {
                case ActionType.AddPanel:
                    action = new AddPanelAction();
                    break;
                case ActionType.LinearPositionLink:
                    action = new LinearPositionLinkAction();
                    break;
                case ActionType.TwoPositionLink:
                    action = new TwoPositionLinkAction();
                    break;
                case ActionType.LoadTool:
                    action = new LoadToolAction();
                    break;
                case ActionType.UnloadTool:
                    action = new UnloadToolAction();
                    break;
                case ActionType.LinearPositionLinkGantryOn:
                    action = new LinearPositionLinkGantryOnAction();
                    break;
                case ActionType.LinearPositionLinkGantryOff:
                    action = new LinearPositionLinkGantryOffAction();
                    break;
                case ActionType.LinearInterpolatedPositionLink:
                    action = new LinearInterpolatedPositionLinkAction();
                    break;
                case ActionType.ArcInterpolatedPositionLink:
                    action = new ArcInterpolatedPositionLinkAction();
                    break;
                case ActionType.ChannelWaiter:
                    action = new ChannelWaiterAction();
                    break;
                case ActionType.NotOperation:
                    action = new NotOperationAction();
                    break;
                default:
                    break;
            }

            if(action != null)
            {
                action.Id = _index++;
                action.Name = $"Action {action.Id}";
                Actions.Add(action);
                MachineStep.Actions.Add(action);
            }
            else
            {
                throw new InvalidOperationException("Action type not supported!");
            }
        }

        private void DelActionCommandImpl()
        {
            MachineStep.Actions.Remove(Selected);
            Actions.Remove(Selected);
            Selected = null;
        }
    }
}
