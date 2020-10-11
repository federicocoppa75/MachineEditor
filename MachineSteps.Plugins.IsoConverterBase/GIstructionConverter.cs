using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using MachineSteps.Models.Actions;
using MachineSteps.Models.Steps;
using MachineSteps.Plugins.IsoIstructions;

namespace MachineSteps.Plugins.IsoConverterBase
{
    public class GIstructionConverter<T> : BaseIstructionConverter<T> where T : class
    {
        public override List<MachineStep> Convert(BaseIstruction istruction, T state)
        {
            return Convert(istruction as GIstruction, state);
        }

        public virtual List<MachineStep> Convert(GIstruction istruction, T state)
        {
            if (istruction == null) throw new ArgumentNullException($"The istruction is not type {typeof(GIstruction).Name}");
            Debug.WriteLine($"Convert method for G{istruction.Istruction} not implemented!");
            return null;
        }


        static protected void SetMaxDuration(MachineStep step)
        {
            if ((step.Actions != null) && (step.Actions.Count > 1))
            {
                var d = step.Actions.Select((a) => (a as LinearPositionLinkAction)?.Duration).Max();
                step.Actions.ForEach((a) => (a as LinearPositionLinkAction).Duration = d.Value);
            }
        }
    }
}
