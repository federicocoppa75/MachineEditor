using MachineSteps.Models.Actions;
using MachineSteps.Models.Steps;
using System.Collections.Generic;
using MachineSteps.Plugins.IsoParser.Attributes;
using MachineSteps.Plugins.IsoIstructions;
using MachineSteps.Plugins.IsoConverterBase;

namespace MachineSteps.Plugins.IsoParser.Converters.G
{
    [GIstructionConverter(692)]
    public class G692Converter : GIstructionConverter<State>
    {
        public override List<MachineStep> Convert(GIstruction istruction, State state)
        {
            if(istruction.Parameters.TryGetValue('X', out double x))
            {
                var step = new MachineStep()
                {
                    Id = GetStepId(),
                    Name = $"G692",
                    Actions = new List<BaseAction>()
                };

                state.Axes.SetRapidX(x, step);
                SetActionsIds(step);
                SetMaxDuration(step);

                return new List<MachineStep>() { step };
            }
            else
            {
                return null;
            }
        }
    }
}
