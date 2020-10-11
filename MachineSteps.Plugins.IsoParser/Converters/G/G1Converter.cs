using System;
using System.Collections.Generic;
using System.Linq;
using MachineSteps.Models.Actions;
using MachineSteps.Models.Steps;
using MachineSteps.Plugins.IsoParser.Attributes;
using MachineSteps.Plugins.IsoIstructions;
using MachineSteps.Plugins.IsoConverterBase;

namespace MachineSteps.Plugins.IsoParser.Converters.G
{
    [GIstructionConverter(1)]
    public class G1Converter : GIstructionConverter<State>
    {
        public override List<MachineStep> Convert(GIstruction istruction, State state)
        {
            var step = new MachineStep()
            {
                Id = GetStepId(),
                Name = "G1",
                Actions = new List<BaseAction>()
            };

            var x = new Nullable<double>();
            var y = new Nullable<double>();
            var z = new Nullable<double>();
            double v = 0.0;

            if (istruction.Parameters.TryGetValue('X', out v)) x = v;
            if (istruction.Parameters.TryGetValue('Y', out v)) y = v;
            if (istruction.Parameters.TryGetValue('Z', out v)) z = v;
            if (istruction.Parameters.TryGetValue('F', out v)) state.FeedSpeed = (int)v;

            state.Axes.SetPosition(step, state.FeedSpeed, x, y, z);

            return (step.Actions.Count() > 0) ? new List<MachineStep>() { step } : null;
        }
    }
}
