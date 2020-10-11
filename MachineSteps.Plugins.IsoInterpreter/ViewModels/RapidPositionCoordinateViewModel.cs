using GalaSoft.MvvmLight;
using MachineSteps.Plugins.IsoInterpreter.Helpers;
using MachineSteps.Plugins.IsoInterpreter.Messages;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace MachineSteps.Plugins.IsoInterpreter.ViewModels
{
    public class RapidPositionCoordinateViewModel : ViewModelBase
    {
        public string Name { get; set; }

        private string _value;

        public string Value
        {
            get { return _value; }
            set
            {
                if(Set(ref _value, value, nameof(Value)))
                {
                    bool computed = false;

                    if (IsComputedValue(_value))
                    {
                        ComputedValue = _value;
                        computed = true;
                    }
                    else
                    {
                        computed = TryToComputeValue(_value);
                    }

                    UpdateStep();
                    IsComputed = computed;
                }
            }
        }

        private string _computedValue;

        public string ComputedValue
        {
            get { return _computedValue; }
            set
            {
                if(Set(ref _computedValue, value, nameof(ComputedValue)))
                {
                    if (IsComputedValue(_computedValue))
                    {
                        AddActiveLevel(_computedValue);
                    }
                    else
                    {
                        ValueOnLevel = "----------";
                    }
                }
            }
        }

        private string _valueOnLevel;

        public string ValueOnLevel
        {
            get { return _valueOnLevel; }
            set { Set(ref _valueOnLevel, value, nameof(ValueOnLevel)); }
        }

        private int _step;

        public int Step
        {
            get => _step;
            set => Set(ref _step, value, nameof(Step));
        }

        public bool IsComputed { get; set; }

        public RapidPositionCoordinateViewModel()
        {
            MessengerInstance.Register<GetAxisValueMessage>(this, OnGetAxisValueMessage);
        }

        public void Add(string value)
        {
            var builder = new StringBuilder();

            builder.AppendFormat("({0})+({1})", Value, value);
            Value = builder.ToString();
        }

        private bool IsComputedValue(string str) => Regex.IsMatch(str, "\\A-?[0-9]+(.[0-9]+)?");

        private bool TryToComputeValue(string value)
        {
            bool result = false;

            if (TryToEvaluateExpression(value, out string computedValue))
            {
                ComputedValue = computedValue;
                AddActiveLevel(computedValue);
                result = true;
            }
            else
            {
                ComputedValue = "----------";
            }            

            return result;
        }


        private bool AddActiveLevel(string value)
        {
            bool result = false;

            if(double.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out double v))
            {
                Action<double, double, double> addOffset = null;

                if((string.Compare(Name, "X") == 0) || (string.Compare(this.Name, "I") == 0))
                {
                    addOffset = (x, y, z) => v += x;
                }
                else if ((string.Compare(Name, "Y") == 0) || (string.Compare(this.Name, "J") == 0))
                {
                    addOffset = (x, y, z) => v += y;
                }
                else if (string.Compare(Name, "Z") == 0)
                {
                    addOffset = (x, y, z) => v += z;
                }

                MessengerInstance.Send(new GetActiveLevelOffsetMessage() { AddOffset = addOffset });

                ValueOnLevel = v.ToString(CultureInfo.InvariantCulture);
            }

            return result;
        }

        private bool TryToEvaluateExpression(string value, out string computedValue)
        {
            var matches = Regex.Matches(value, $"({ExpressionHelper.VlVariableStringMatch})|({ExpressionHelper.ExkVariableStringMatch})|({ExpressionHelper.AxisParameterStringMatch})");
            var varValues = new Dictionary<string, double>();
            var varReadys = new Dictionary<string, bool>();
            bool result = false;

            computedValue = string.Empty;

            for (int i = 0; i < matches.Count; i++)
            {
                var vName = matches[i].Value;
                double vValue = 0.0;
                bool vReady = false;
                bool vFound = false;

                MessengerInstance.Send(new GetVariableValueMessage()
                {
                    Name = vName,
                    SetValue = (d) => vValue = d,
                    SetReady = (b) => vReady = b,
                    SetFinded = () => vFound = true
                });             

                varValues[vName] = vValue;
                varReadys[vName] = vReady;
                if (!vFound) NotifyNotFoundParameter(vName);
            }

            if (varReadys.Values.All(b => b))
            {
                var expression = value;

                foreach (var item in varValues)
                {
                    expression = expression.Replace(item.Key, item.Value.ToString());
                }

                expression = expression.Replace("%", "");
                expression = expression.Replace("+-", "-");
                expression = expression.Replace("--", "+");
                expression = expression.Replace(",", ".");

                if (ExpressionHelper.EvaluateExpression(expression, out object expressionResult))
                {
                    if(ExpressionHelper.GetComputeValueAsString(expressionResult, out computedValue))
                    {
                        result = true;
                    }                    
                }
            }

            return result;
        }

        private void OnGetAxisValueMessage(GetAxisValueMessage msg)
        {
            if(string.Compare(Name, msg.AxisName) == 0)
            {
                if(IsComputedValue(_valueOnLevel))
                {
                    if(double.TryParse(_valueOnLevel, NumberStyles.Any, CultureInfo.InvariantCulture, out double v))
                    {
                        msg.SetReady?.Invoke(true);
                        msg.SetValue?.Invoke(v);
                    }
                }

                msg.SetFinded?.Invoke();
            }
        }

        private void UpdateStep() => MessengerInstance.Send(new GetSelectedStepMessage() { SetStep = (s) => Step = s.Number });

        private void NotifyNotFoundParameter(string name) => MessengerInstance.Send(new NotifyNotFoundParameterMessage() { Name = name });

    }
}
