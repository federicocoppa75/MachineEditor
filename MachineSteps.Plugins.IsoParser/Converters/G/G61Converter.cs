using MachineSteps.Plugins.IsoConverterBase;
using MachineSteps.Plugins.IsoParser.Attributes;

namespace MachineSteps.Plugins.IsoParser.Converters.G
{
    [GIstructionConverter(61)]
    public class G61Converter : GIstructionConverter<State>
    {
    }
}
