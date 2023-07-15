using GalaSoft.MvvmLight;
using MachineSteps.Plugins.IsoInterpreter.Helpers;
using MachineSteps.Plugins.IsoInterpreter.Messages;
using MachineSteps.Plugins.IsoInterpreter.Messages.Conversion;
using System;
using System.Globalization;

namespace MachineSteps.Plugins.IsoInterpreter.ViewModels
{
    public class VariableViewModel : ViewModelBase
    {
        public string Name { get; set; }

        private string _value;

        public string Value
        {
            get => _value;
            set
            {
                bool changed = Set(ref _value, value, nameof(Value));
                bool computed = false;

                if(ExpressionHelper.IsComputedValue(_value))
                {
                    ComputedValue = _value;
                    computed = true;
                }
                else
                {
                    computed = TryToComputeValue(_value);
                }

                UpdateStep();
                if(EnableNotifyChange && computed) NotifyChange();
            }
        }

        private string _computedValue;

        public string ComputedValue
        {
            get => _computedValue; 
            set => Set(ref _computedValue, value, nameof(ComputedValue)); 
        }

        private int _step;

        public int Step
        {
            get => _step;
            set => Set(ref _step, value, nameof(Step));
        }

        public bool EnableNotifyChange { get; set; }

        public VariableViewModel()
        {
            MessengerInstance.Register<GetVariableValueMessage>(this, OnGetVariableValueMessage);
        }

        private bool TryToComputeValue(string value)
        {
            bool result = false;
            object expressionResult = null;

            if (ExpressionHelper.TryToEvaluateExpression(Name, value, ComputedValue, o => expressionResult = o))
            {
                if(ExpressionHelper.GetComputeValueAsString(expressionResult, out string s))
                {
                    ComputedValue = s;
                    result = true;
                }                
            }
            else
            {
                ComputedValue = "----------";
            }            

            return result;
        }

        private void OnGetVariableValueMessage(GetVariableValueMessage msg)
        {
            if(string.Compare(Name, msg.Name) == 0)
            {
                var isComputed = ExpressionHelper.IsComputedValue(_computedValue);

                if(isComputed)
                {
                    isComputed = double.TryParse(_computedValue, NumberStyles.Any, CultureInfo.InvariantCulture, out double value);

                    if (isComputed) msg.SetValue?.Invoke(value);
                }

                msg.SetReady?.Invoke(isComputed);
                msg.SetFinded?.Invoke();
            }
        }

        private void UpdateStep() => MessengerInstance.Send(new GetSelectedStepMessage() { SetStep = (s) => Step = s.Number });

        private void NotifyChange()
        {
            MessengerInstance.Send(new SetVariableMessage()
            {
                Step = Step,
                Name = Name,
                Value = ComputedValue
            });
        }
    }
}
