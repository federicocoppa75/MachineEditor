using System.Collections.Generic;

namespace MachineSteps.Plugins.IsoIstructions
{
    public class MultipleMachIstructions : MachIstruction
    {
        public IEnumerable<MachIstruction> Istructions { get; set; }
    }
}
