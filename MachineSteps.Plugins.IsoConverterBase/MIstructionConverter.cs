using System;
using System.Collections.Generic;
using System.Diagnostics;
using MachineSteps.Models.Steps;
using MachineSteps.Plugins.IsoIstructions;

namespace MachineSteps.Plugins.IsoConverterBase
{
    public class MIstructionConverter<T> : MachIstructionConverter<T> where T : class
    {
        public override List<MachineStep> Convert(BaseIstruction istruction, T state)
        {
            return Convert(istruction as MIstruction, state);
        }

        public virtual List<MachineStep> Convert(MIstruction istruction, T state)
        {
            if (istruction == null) throw new ArgumentNullException($"The istruction is not type {typeof(MIstruction).Name}");
            Debug.WriteLine($"Convert method for M{istruction.Istruction} not implemented!");
            return null;
        }
    }
}
