using MachineSteps.Plugins.IsoConverterBase;
using MachineSteps.Plugins.IsoParser.Attributes;

namespace MachineSteps.Plugins.IsoParser.Converters.EDK
{
    [EDKConverter(15)]
    public class EDK15Converter : SetVariableIstructionConverter<State>
    {
    }
}
