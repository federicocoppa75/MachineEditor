using MachineSteps.Plugins.IsoConverterBase;
using MachineSteps.Plugins.IsoParser.Attributes;

namespace MachineSteps.Plugins.IsoParser.Converters.EDK
{
    [EDKConverter(1)]
    public class EDK1Converter : SetVariableIstructionConverter<State>
    {
    }
}
