using MachineSteps.Models.Steps;
using System.Collections.Generic;
using MachineSteps.Plugins.IsoParser.Attributes;
using MachineSteps.Plugins.IsoIstructions;
using MachineSteps.Plugins.IsoConverterBase;

namespace MachineSteps.Plugins.IsoParser.Converters.M
{
    [MIstructionConverter(33)]
    public class M33Converter : MIstructionConverter<State>
    {
        public override List<MachineStep> Convert(MIstruction istruction, State state)
        {
            state.SelectedHead = 1;
            state.HeadOrder = 2;

            return null;
        }
    }
}
