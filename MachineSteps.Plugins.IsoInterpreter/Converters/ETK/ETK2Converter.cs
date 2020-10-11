using System.Collections;
using MachineSteps.Plugins.IsoConverterBase;
using MachineSteps.Plugins.IsoInterpreter.Attributes;

namespace MachineSteps.Plugins.IsoInterpreter.Converters.ETK
{
    [ETKConverter(2)]
    public class ETK2Converter : SetVariableIstructionConverter<State>
    {
    }
}
