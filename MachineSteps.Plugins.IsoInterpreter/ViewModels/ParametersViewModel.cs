using GalaSoft.MvvmLight;
using MachineSteps.Plugins.IsoInterpreter.Messages;
using MachineSteps.Plugins.IsoInterpreter.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;

namespace MachineSteps.Plugins.IsoInterpreter.ViewModels
{
    public abstract class ParametersViewModel : ViewModelBase
    {
        public ObservableCollection<ParameterViewModel> Parameters { get; set; } = new ObservableCollection<ParameterViewModel>();

        public ParametersViewModel()
        {
            MessengerInstance.Register<NotifyNotFoundParameterMessage>(this, OnNotifyNotFoundParameterMessage);
        }

        protected abstract bool IsParameterManaged(string name);

        private void OnNotifyNotFoundParameterMessage(NotifyNotFoundParameterMessage msg)
        {
            if(IsParameterManaged(msg.Name))
            {
                var vm = Parameters.FirstOrDefault(m => string.Compare(m.Name, msg.Name) == 0);

                if(vm == null)
                {
                    Parameters.Add(new ParameterViewModel() { Name = msg.Name, Value = "---------" });
                }
            }
        }

        protected List<Parameter> ToModel()
        {
            var parameters = new List<Parameter>();

            foreach (var item in Parameters)
            {
                if(double.TryParse(item.Value, System.Globalization.NumberStyles.Any, CultureInfo.InvariantCulture, out double v))
                {
                    parameters.Add(new Parameter() { Name = item.Name, Value = item.Value });
                }                
            }

            return parameters;
        }

        protected void FromModel(List<Parameter> parameters)
        {
            foreach (var item in parameters)
            {
                Parameters.Add(new ParameterViewModel() { Name = item.Name, Value = item.Value });
            }
        }
    }
}
