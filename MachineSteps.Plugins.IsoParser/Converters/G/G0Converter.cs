using System.Collections.Generic;
using System.Linq;
using MachineSteps.Models.Actions;
using MachineSteps.Models.Steps;
using MachineSteps.Plugins.IsoParser.Attributes;
using MachineSteps.Plugins.IsoIstructions;
using MachineSteps.Plugins.IsoConverterBase;

namespace MachineSteps.Plugins.IsoParser.Converters.G
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
                if(state.Axes.M.IsIdentity() || state.Axes.M.IsIdentityFlipped())
                {
                    if (istruction.Parameters.TryGetValue('X', out double x)) state.Axes.SetRapidX(x, step);
                    if (istruction.Parameters.TryGetValue('Y', out double y)) state.Axes.SetRapidY(y, step);
                    if (istruction.Parameters.TryGetValue('Z', out double z)) state.Axes.SetRapidZ(z, step);
                }
                else
                {
                    double? nx = null;
                    double? ny = null;
                    double? nz = null;

                    if (istruction.Parameters.TryGetValue('X', out double x)) nx = x;
                    if (istruction.Parameters.TryGetValue('Y', out double y)) ny = y;
                    if (istruction.Parameters.TryGetValue('Z', out double z)) nz = z;

                    state.Axes.SetRapid(nx, ny, nz, step);
                }

                SetActionsIds(step);
                SetMaxDuration(step);
            }

            return (step.Actions.Count() > 0) ? new List<MachineStep>() { step } : null;
        }
    }
}
