using GalaSoft.MvvmLight;
using MachineSteps.Plugins.IsoInterpreter.Messages;
using System.Globalization;

namespace MachineSteps.Plugins.IsoInterpreter.ViewModels
{
    public class ParameterViewModel : ViewModelBase
    {
        public string Name { get; set; }

        private string _value;

        public string Value
        {
            get => _value; 
            set => Set(ref _value, value, nameof(Value));
        }

        public ParameterViewModel()
        {
            MessengerInstance.Register<GetVariableValueMessage>(this, OnGetVariableValueMessage);
        }

        private void OnGetVariableValueMessage(GetVariableValueMessage msg)
        {
            if(string.Compare(Name, msg.Name) == 0)
            {
                if(double.TryParse(_value, NumberStyles.Any, CultureInfo.InvariantCulture, out double v))
                {
                    msg.SetValue?.Invoke(v);
                    msg.SetReady?.Invoke(true);
                }

                msg.SetFinded?.Invoke();
            }
        }
    }
}
