using MachineSteps.Plugins.IsoConverterBase;
using MachineSteps.Plugins.IsoInterpreter.Attributes;

namespace MachineSteps.Plugins.IsoInterpreter.Converters.M
{
    [MIstructionConverter(20)]
    public class M20Converter : MIstructionConverter<State>
    {
    }
}
