using MachineSteps.Plugins.IsoConverterBase;
using MachineSteps.Plugins.IsoInterpreter.Attributes;

namespace MachineSteps.Plugins.IsoInterpreter.Converters.EDK
{
    [EDKConverter(1)]
    public class EDK1Converter : SetVariableIstructionConverter<State>
    {
    }
}
