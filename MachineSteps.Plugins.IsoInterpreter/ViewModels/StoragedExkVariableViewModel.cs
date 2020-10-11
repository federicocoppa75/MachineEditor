using MachineSteps.Plugins.IsoInterpreter.Helpers;
using MachineSteps.Plugins.IsoInterpreter.Messages;
using System.Text.RegularExpressions;

namespace MachineSteps.Plugins.IsoInterpreter.ViewModels
{
    public class StoragedExkVariableViewModel : ParametersViewModel
    {
        public StoragedExkVariableViewModel() : base()
        {
            MessengerInstance.Register<GetStoragedExkParametersMessage>(this, OnGetStoragedExkParametersMessage);
            MessengerInstance.Register<SetStoragedExkParametersMessage>(this, OnSetStoragedExkParametersMessage);
        }

        protected override bool IsParameterManaged(string name) => Regex.IsMatch(name, ExpressionHelper.ExkVariableStringMatch);

        private void OnSetStoragedExkParametersMessage(SetStoragedExkParametersMessage msg) => FromModel(msg.Parameters);

        private void OnGetStoragedExkParametersMessage(GetStoragedExkParametersMessage msg) => msg.GetParameters?.Invoke(ToModel());
    }
}
