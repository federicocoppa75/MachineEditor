using MachineSteps.Plugins.IsoConverterBase;
using MachineSteps.Plugins.IsoParser.Attributes;

namespace MachineSteps.Plugins.IsoParser.Converters.ETK
{
    [ETKConverter(204)]
    public class ETK204Converter : SetVariableIstructionConverter<State>
    {
    }
}
