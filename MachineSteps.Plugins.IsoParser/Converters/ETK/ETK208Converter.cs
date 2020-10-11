using MachineSteps.Plugins.IsoConverterBase;
using MachineSteps.Plugins.IsoParser.Attributes;

namespace MachineSteps.Plugins.IsoParser.Converters.ETK
{
    [ETKConverter(208)]
    public class ETK208Converter : SetVariableIstructionConverter<State>
    {
    }
}
