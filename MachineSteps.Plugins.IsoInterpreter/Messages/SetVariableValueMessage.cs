using MachineSteps.Plugins.IsoInterpreter.Enums;

namespace MachineSteps.Plugins.IsoInterpreter.Messages
{
    public class SetVariableValueMessage
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public VariableType VariableType { get; set; }
    }
}
