using MachineSteps.Plugins.IsoConverterBase;
using MachineSteps.Plugins.IsoParser.Attributes;

namespace MachineSteps.Plugins.IsoParser.Converters.EOK
{
    [EOKConverter(238)]
    public class EOK238Converter : SetVariableIstructionConverter<State>
    {
    }
}
