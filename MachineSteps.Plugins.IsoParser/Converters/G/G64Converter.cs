using MachineSteps.Plugins.IsoConverterBase;
using MachineSteps.Plugins.IsoParser.Attributes;

namespace MachineSteps.Plugins.IsoParser.Converters.G
{
    [GIstructionConverter(64)]
    public class G64Converter : GIstructionConverter<State>
    {
    }
}
