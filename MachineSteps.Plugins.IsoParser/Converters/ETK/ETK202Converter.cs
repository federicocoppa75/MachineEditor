using MachineSteps.Models.Steps;
using System.Collections.Generic;
using MachineSteps.Plugins.IsoIstructions;
using MachineSteps.Plugins.IsoParser.Attributes;
using MachineSteps.Plugins.IsoConverterBase;

namespace MachineSteps.Plugins.IsoParser.Converters.ETK
{
    [ETKConverter(202)]
    public class ETK202Converter : SetVariableIstructionConverter<State>
    {
        public override List<MachineStep> Convert(SetVariableIstruction istruction, State state)
        {
            state.ToolChange.ToolChangeSteps[0].SourcePosition = (int)istruction.Value;

            return null;
        }
    }
}
