using System.Collections;
using MachineSteps.Plugins.IsoConverterBase;
using MachineSteps.Plugins.IsoInterpreter.Attributes;

namespace MachineSteps.Plugins.IsoInterpreter.Converters.ETK
{
    [ETKConverter(18)]
    public class ETK18Converter : SetVariableIstructionConverter<State>
    {
    }
}
