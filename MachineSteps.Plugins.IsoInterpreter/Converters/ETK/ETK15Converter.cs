using MachineSteps.Plugins.IsoConverterBase;
using MachineSteps.Plugins.IsoInterpreter.Attributes;
using System.Collections;

namespace MachineSteps.Plugins.IsoInterpreter.Converters.ETK
{
    [ETKConverter(15)]
    public class ETK15Converter : SetVariableIstructionConverter<State>
    {
    }
}
