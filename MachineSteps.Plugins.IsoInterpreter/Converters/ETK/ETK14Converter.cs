using MachineSteps.Plugins.IsoConverterBase;
using MachineSteps.Plugins.IsoInterpreter.Attributes;

namespace MachineSteps.Plugins.IsoInterpreter.Converters.ETK
{
    [ETKConverter(14)]
    public class ETK14Converter : SetVariableIstructionConverter<State>
    {
    }
}
