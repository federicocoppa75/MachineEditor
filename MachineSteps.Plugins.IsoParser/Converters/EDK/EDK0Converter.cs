using MachineSteps.Plugins.IsoConverterBase;
using MachineSteps.Plugins.IsoParser.Attributes;

namespace MachineSteps.Plugins.IsoParser.Converters.EDK
{
    [EDKConverter(0)]
    public class EDK0Converter : SetVariableIstructionConverter<State>
    {
    }
}
