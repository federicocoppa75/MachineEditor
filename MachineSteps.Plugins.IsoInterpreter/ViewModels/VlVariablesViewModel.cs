using MachineSteps.Plugins.IsoInterpreter.Enums;

namespace MachineSteps.Plugins.IsoInterpreter.ViewModels
{
    public class VlVariablesViewModel : VariablesViewModel
    {
        protected override bool IsVariableManaged(VariableType type) => type == VariableType.VL;

        public VlVariablesViewModel() : base()
        {
        }
    }
}
