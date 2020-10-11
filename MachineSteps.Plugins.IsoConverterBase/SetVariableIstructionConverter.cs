using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MachineSteps.Models.Steps;
using MachineSteps.Plugins.IsoIstructions;

namespace MachineSteps.Plugins.IsoConverterBase
{
    public class SetVariableIstructionConverter<T> : BaseIstructionConverter<T> where T : class
    {
        public override List<MachineStep> Convert(BaseIstruction istruction, T state)
        {
            return Convert(istruction as SetVariableIstruction, state);
        }

        public virtual List<MachineStep> Convert(SetVariableIstruction istruction, T state)
        {
            if (istruction == null) throw new ArgumentNullException($"The istruction is not type {typeof(SetVariableIstruction).Name}");
            Debug.WriteLine($"Convert method for variable {istruction.Name}{istruction.Index} not implemented!");
            return null;
        }
    }
}
