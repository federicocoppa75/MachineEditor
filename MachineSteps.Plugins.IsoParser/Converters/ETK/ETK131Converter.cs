using MachineSteps.Plugins.IsoConverterBase;
using MachineSteps.Plugins.IsoParser.Attributes;

namespace MachineSteps.Plugins.IsoParser.Converters.ETK
{
    [ETKConverter(131)]
    public class ETK131Converter : SetVariableIstructionConverter<State>
    {
    }
}
