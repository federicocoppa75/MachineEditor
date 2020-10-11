using System.Collections.Generic;
using MachineSteps.Models.Steps;
using MachineSteps.Plugins.IsoParser.Attributes;
using MachineSteps.Plugins.IsoIstructions;
using MachineSteps.Plugins.IsoConverterBase;

namespace MachineSteps.Plugins.IsoParser.Converters.EOK
{
    [EOKConverter(213)]
    public class EOK213Converter : SetVariableIstructionConverter<State>
    {
        public override List<MachineStep> Convert(SetVariableIstruction istruction, State state)
        {
            state.Panel.Dz = istruction.Value;
            return null;
        }
    }
}
