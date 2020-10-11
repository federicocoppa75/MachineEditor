using System.Collections.Generic;
using System.Linq;
using MachineSteps.Models.Actions;
using MachineSteps.Models.Steps;
using MachineSteps.Plugins.IsoConverterBase;
using MachineSteps.Plugins.IsoInterpreter.Attributes;
using MachineSteps.Plugins.IsoIstructions;

namespace MachineSteps.Plugins.IsoInterpreter.Converters.G
{
    [GIstructionConverter(0)]
    public class G0Converter : GIstructionConverter<State>
    {
        public override List<MachineStep> Convert(GIstruction istruction, State state)
        {
            var step = new MachineStep()
            {
                Id = GetStepId(),
                Name = $"G0",
                Actions = new List<BaseAction>()
            };

            if (istruction.Parameters != null && istruction.Parameters.Count > 0)
            {
                if (istruction.Parameters.TryGetValue('X', out double x)) state.Axes.SetX(x, step);
                if (istruction.Parameters.TryGetValue('Y', out double y)) state.Axes.SetY(y, step);
                if (istruction.Parameters.TryGetValue('Z', out double z)) state.Axes.SetZ(z, step);

                SetActionsIds(step);
                SetMaxDuration(step);
            }

            return (step.Actions.Count() > 0) ? new List<MachineStep>() { step } : null;
        }
    }
}
