using System.Collections.Generic;
using MachineSteps.Models.Steps;
using MachineSteps.Plugins.IsoParser.Attributes;
using MachineSteps.Plugins.IsoIstructions;
using MachineSteps.Plugins.IsoConverterBase;

namespace MachineSteps.Plugins.IsoParser.Converters.M
{
    [MIstructionConverter(34)]
    public class M34Converter : MIstructionConverter<State>
    {
        public override List<MachineStep> Convert(MIstruction istruction, State state)
        {
            state.SelectedHead = 1;
            state.HeadOrder = 1;

            return null;
        }
    }
}
