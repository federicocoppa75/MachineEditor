using System.Collections.Generic;
using MachineSteps.Models.Steps;
using MachineSteps.Plugins.IsoParser.Attributes;
using MachineSteps.Plugins.IsoIstructions;
using MachineSteps.Plugins.IsoConverterBase;

namespace MachineSteps.Plugins.IsoParser.Converters.G
{
    [GIstructionConverter(310)]
    public class G310Converter : GIstructionConverter<State>
    {
        public override List<MachineStep> Convert(GIstruction istruction, State state)
        {
            if (istruction.Parameters.TryGetValue('L', out double length)) state.Axes.L = length;
            if (istruction.Parameters.TryGetValue('R', out double radius)) state.Axes.R = radius;

            return null;
        }
    }
}
