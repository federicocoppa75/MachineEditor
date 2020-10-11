using System.Collections;
using MachineSteps.Plugins.IsoConverterBase;
using MachineSteps.Plugins.IsoInterpreter.Attributes;

namespace MachineSteps.Plugins.IsoInterpreter.Converters.ETK
{
    [ETKConverter(17)]
    public class ETK17Converter : SetVariableIstructionConverter<State>
    {
    }
}
