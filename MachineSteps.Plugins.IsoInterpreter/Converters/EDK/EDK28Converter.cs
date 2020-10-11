using MachineSteps.Plugins.IsoConverterBase;
using MachineSteps.Plugins.IsoInterpreter.Attributes;

namespace MachineSteps.Plugins.IsoInterpreter.Converters.EDK
{
    [EDKConverter(28)]
    public class EDK28Converter : SetVariableIstructionConverter<State>
    {
    }
}
