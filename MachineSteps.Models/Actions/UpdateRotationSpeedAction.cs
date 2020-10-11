using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineSteps.Models.Actions
{
    [Serializable]
    public class UpdateRotationSpeedAction : BaseAction
    {
        public int NewRotationSpeed { get; set; }
        public int OldRotationSpeed { get; set; }

        public double Duration { get; set; }
    }
}
