using System.Collections.Generic;
using MachineSteps.Models.Steps;
using MachineSteps.Plugins.IsoParser.Attributes;
using MachineSteps.Plugins.IsoIstructions;
using MachineSteps.Plugins.IsoConverterBase;

namespace MachineSteps.Plugins.IsoParser.Converters.ETK
{
    [ETKConverter(199)]
    public class ETK199Converter : SetVariableIstructionConverter<State>
    {
        public override List<MachineStep> Convert(SetVariableIstruction istruction, State state)
        {
            state.ToolChange.StartIndex = (int)istruction.Value;

            return null;
        }
    }
}
