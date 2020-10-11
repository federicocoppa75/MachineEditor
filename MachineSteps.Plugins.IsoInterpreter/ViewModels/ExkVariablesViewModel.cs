using MachineSteps.Plugins.IsoInterpreter.Enums;

namespace MachineSteps.Plugins.IsoInterpreter.ViewModels
{
    public class ExkVariablesViewModel : VariablesViewModel
    {
        protected override bool IsVariableManaged(VariableType type) => type == VariableType.ExK;

        public ExkVariablesViewModel() : base()
        {
        }
    }
}
