using MachineSteps.Plugins.IsoConverterBase;
using MachineSteps.Plugins.IsoInterpreter.Attributes;

namespace MachineSteps.Plugins.IsoInterpreter.Converters.ETK
{
    [ETKConverter(714)]
    public class ETK714Converter : SetVariableIstructionConverter<State>
    {
    }
}
