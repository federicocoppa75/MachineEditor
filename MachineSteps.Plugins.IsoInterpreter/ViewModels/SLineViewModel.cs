using GalaSoft.MvvmLight;
using MachineSteps.Plugins.IsoInterpreter.Messages;
using MachineSteps.Plugins.IsoInterpreter.Messages.Conversion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineSteps.Plugins.IsoInterpreter.ViewModels
{
    public class SLineViewModel : ViewModelBase
    {
        private bool _notifyChange = false;

        private int _value;

        public int Value
        {
            get => _value;
            set
            {
                if(Set(ref _value, value, nameof(Value)))
                {
                    UpdateStep();
                    if(_notifyChange) NotifyChange();
                }
            }
        }

        private int _step;

        public int Step
        {
            get => _step;
            set => Set(ref _step, value, nameof(Step));
        }

        public SLineViewModel() : base()
        {
            MessengerInstance.Register<SLineMessage>(this, OnSLineMessage);
            MessengerInstance.Register<IstructionListenerSwitchOnMessage>(this, (m) => _notifyChange = true);
            MessengerInstance.Register<IstructionListenerSwitchOffMessage>(this, (m) => _notifyChange = false);

        }

        private void UpdateStep() => MessengerInstance.Send(new GetSelectedStepMessage() { SetStep = (s) => Step = s.Number });


        private void OnSLineMessage(SLineMessage msg)
        {
            Value = msg.Value;
        }

        private void NotifyChange()
        {
            MessengerInstance.Send(new SIstructionMessage()
            {
                Value = Value,
                Step = Step
            });
        }
    }
}
