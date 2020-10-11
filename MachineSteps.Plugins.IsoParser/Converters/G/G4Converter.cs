using MachineSteps.Plugins.IsoConverterBase;
using MachineSteps.Plugins.IsoParser.Attributes;

namespace MachineSteps.Plugins.IsoParser.Converters.G
{
    [GIstructionConverter(4)]
    public class G4Converter : GIstructionConverter<State>
    {
    }
}
