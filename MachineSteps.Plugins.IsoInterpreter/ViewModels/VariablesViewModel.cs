using GalaSoft.MvvmLight;
using MachineSteps.Plugins.IsoInterpreter.Enums;
using MachineSteps.Plugins.IsoInterpreter.Messages;
using MachineSteps.Plugins.IsoInterpreter.Messages.Conversion;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace MachineSteps.Plugins.IsoInterpreter.ViewModels
{
    public abstract class VariablesViewModel : ViewModelBase
    {
        private bool _notifyChange;

        public ObservableCollection<VariableViewModel> Variables { get; set; } = new ObservableCollection<VariableViewModel>();

        public VariablesViewModel()
        {
            MessengerInstance.Register<SetVariableValueMessage>(this, OnSetVariableValueMessage);
            MessengerInstance.Register<IsoLineSelectionChangedMessage>(this, OnIsoLineSelectionChangedMessage);
            MessengerInstance.Register<FlushIsoLinesMessage>(this, OnFlushIsoLinesMessage);
            MessengerInstance.Register<IstructionListenerSwitchOnMessage>(this, (m) => NotifyChange(true));
            MessengerInstance.Register<IstructionListenerSwitchOffMessage>(this, (m) => NotifyChange(false));
        }

        private void OnSetVariableValueMessage(SetVariableValueMessage msg)
        {
            if (IsVariableManaged(msg.VariableType))
            {
                var vm = Variables.FirstOrDefault((v) => string.Compare(v.Name, msg.Name) == 0);

                if (vm != null)
                {
                    vm.Value = msg.Value;
                }
                else
                {
                    Variables.Add(new VariableViewModel()
                    {
                        Name = msg.Name,
                        EnableNotifyChange = _notifyChange,
                        Value = msg.Value
                    });
                }
            }
        }

        private void OnIsoLineSelectionChangedMessage(IsoLineSelectionChangedMessage obj)
        {
            Variables.Clear();
        }
        
        private void OnFlushIsoLinesMessage(FlushIsoLinesMessage obj)
        {
            Variables.Clear();
        }

        protected abstract bool IsVariableManaged(VariableType type);

        private void NotifyChange(bool v)
        {
            _notifyChange = v;

            foreach (var item in Variables) item.EnableNotifyChange = v;
        }

    }
}
