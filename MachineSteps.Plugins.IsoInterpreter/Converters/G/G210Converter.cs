using MachineSteps.Models.Actions;
using MachineSteps.Models.Steps;
using MachineSteps.Plugins.IsoConverterBase;
using MachineSteps.Plugins.IsoInterpreter.Attributes;
using MachineSteps.Plugins.IsoIstructions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineSteps.Plugins.IsoInterpreter.Converters.G
{
    [GIstructionConverter(210)]
    public class G210Converter : GIstructionConverter<State>
    {
        public override List<MachineStep> Convert(GIstruction istruction, State state)
        {
            var step = new MachineStep()
            {
                Id = GetStepId(),
                Name = $"G210",
                Actions = new List<BaseAction>()
            };

            var x = new Nullable<double>();
            double v = 0.0;

            if (istruction.Parameters.TryGetValue('X', out v)) x = v;

            state.Axes.SetPosition(step, 500, x, null, null);
            SetActionsIds(step);

            return (step.Actions.Count() > 0) ? new List<MachineStep>() { step } : null;
        }
    }
}
