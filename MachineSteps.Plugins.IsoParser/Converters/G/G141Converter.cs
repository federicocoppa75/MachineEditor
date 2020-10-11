using System.Collections.Generic;
using MachineSteps.Models.Actions;
using MachineSteps.Models.Steps;
using MachineSteps.Plugins.IsoParser.Attributes;
using MachineSteps.Plugins.IsoIstructions;
using MachineSteps.Plugins.IsoConverterBase;

namespace MachineSteps.Plugins.IsoParser.Converters.G
{
    [GIstructionConverter(141)]
    public class G141Converter : GIstructionConverter<State>
    {
        public override List<MachineStep> Convert(GIstruction istruction, State state)
        {
            if(istruction.Parameters.TryGetValue('G', out double g))
            {
                var step = new MachineStep()
                {
                    Id = GetStepId(),
                    Name = $"G141 G{g}",
                    Actions = new List<BaseAction>()
                };

                if(g == 1.0)
                {
                    state.Axes.ResetGantryX(step);
                }
                else if(g == 2.0)
                {
                    state.Axes.ResetGantryY(step);
                }
                else if(g == 3.0)
                {
                    state.Axes.ResetGantryZ(step);
                }
                else if (g == 4.0)
                {
                    state.Axes.ResetGantryZ2(step);
                }
                else
                {
                    state.Axes.ResetGantryX(step);
                    state.Axes.ResetGantryY(step);
                    state.Axes.ResetGantryZ(step);
                    state.Axes.ResetGantryZ2(step);
                }

                SetActionsIds(step);

                return (step.Actions.Count > 0) ? new List<MachineStep>() { step } : null;
            }
            else
            {
                return null;
            }
        }
    }
}
