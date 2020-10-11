using System.Collections.Generic;
using MachineSteps.Models.Steps;
using MachineSteps.Plugins.IsoParser.Attributes;
using MachineSteps.Plugins.IsoIstructions;
using MachineSteps.Plugins.IsoConverterBase;

namespace MachineSteps.Plugins.IsoParser.Converters.G
{
    [GIstructionConverter(161)]
    public class G161Converter : GIstructionConverter<State>
    {
        public override List<MachineStep> Convert(GIstruction istruction, State state)
        {
            state.Axes.OX = istruction.Parameters['X'];
            state.Axes.OY = istruction.Parameters['Y'];
            state.Axes.OZ = istruction.Parameters['Z'];

            return null;
        }
    }
}
