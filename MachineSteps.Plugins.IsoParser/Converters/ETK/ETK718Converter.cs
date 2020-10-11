using MachineSteps.Plugins.IsoConverterBase;
using MachineSteps.Plugins.IsoParser.Attributes;

namespace MachineSteps.Plugins.IsoParser.Converters.ETK
{
    [ETKConverter(718)]
    public class ETK718Converter : SetVariableIstructionConverter<State>
    {
    }
}
