using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineSteps.Models.Actions
{
    [Serializable]
    public class InjectAction : BaseAction
    {
        public int InjectorId { get; set; }

        public double Duration { get; set; }
    }
}
