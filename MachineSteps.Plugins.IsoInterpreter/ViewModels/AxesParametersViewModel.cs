using MachineSteps.Plugins.IsoInterpreter.Helpers;
using MachineSteps.Plugins.IsoInterpreter.Messages;
using System.Text.RegularExpressions;

namespace MachineSteps.Plugins.IsoInterpreter.ViewModels
{
    public class AxesParametersViewModel : ParametersViewModel
    {
        public AxesParametersViewModel() : base()
        {
            MessengerInstance.Register<GetAxesParametersMessage>(this, OnGetAxesParametersMessage);
            MessengerInstance.Register<SetAxesParametersMessage>(this, OnSetAxesParametersMessage);
        }

        protected override bool IsParameterManaged(string name) => Regex.IsMatch(name, ExpressionHelper.AxisParameterStringMatch);

        private void OnSetAxesParametersMessage(SetAxesParametersMessage msg) => FromModel(msg.Parameters);

        private void OnGetAxesParametersMessage(GetAxesParametersMessage msg) => msg.GetParameters?.Invoke(ToModel());

    }
}
