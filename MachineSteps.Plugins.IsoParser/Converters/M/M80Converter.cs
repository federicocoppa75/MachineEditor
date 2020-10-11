using MachineSteps.Plugins.IsoConverterBase;
using MachineSteps.Plugins.IsoParser.Attributes;

namespace MachineSteps.Plugins.IsoParser.Converters.M
{
    [MIstructionConverter(80)]
    public class M80Converter : MIstructionConverter<State>
    {
    }
}
