using MachineSteps.Models.Actions;
using MachineSteps.Models.Steps;
using MachineSteps.Plugins.IsoConverterBase;
using MachineSteps.Plugins.IsoIstructions;
using MachineSteps.Plugins.IsoParser.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineSteps.Plugins.IsoParser.Converters.G
{
    [GIstructionConverter(210)]
    public class G210Converter : GIstructionConverter<State>
    {
        public override List<MachineStep> Convert(GIstruction istruction, State state)
        {
            var step = new MachineStep()
            {
                Id = GetStepId(),
                Name = "G210",
                Actions = new List<BaseAction>()
            };

            var x = new Nullable<double>();
            var y = new Nullable<double>();
            var z = new Nullable<double>();

            if (istruction.Parameters.TryGetValue('X', out double v)) x = v;
            if (istruction.Parameters.TryGetValue('Y', out v)) y = v;
            if (istruction.Parameters.TryGetValue('Z', out v)) z = v;

            // la tastatura azzera la G161
            state.Axes.OX = 0.0;
            state.Axes.OY = 0.0;
            //state.Axes.OZ = 0.0; // dalle prove fatte sembra che sulla Z debba rimanere l'effetto G161 (???)

            state.Axes.SetPosition(step, 500, x, y, z);

            return (step.Actions.Count() > 0) ? new List<MachineStep>() { step } : null;
        }
    }
}
