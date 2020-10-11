using MachineSteps.Plugins.IsoConverterBase;
using MachineSteps.Plugins.IsoParser.Attributes;

namespace MachineSteps.Plugins.IsoParser.Converters.EDK
{
    [EDKConverter(11)]
    public class EDK11Converter : SetVariableIstructionConverter<State>
    {
    }
}
