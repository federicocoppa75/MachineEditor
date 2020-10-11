using MachineSteps.Models.Steps;
using System.Collections.Generic;
using MachineSteps.Plugins.IsoParser.Attributes;
using MachineSteps.Plugins.IsoIstructions;
using MachineSteps.Plugins.IsoConverterBase;

namespace MachineSteps.Plugins.IsoParser.Converters.EOK
{
    [EOKConverter(212)]
    public class EOK212Converter : SetVariableIstructionConverter<State>
    {
        public override List<MachineStep> Convert(SetVariableIstruction istruction, State state)
        {
            state.Panel.Dy = istruction.Value;
            return null;
        }
    }
}
