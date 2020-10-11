using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineSteps.Models.Actions
{
    [Serializable]
    public class TurnOffInverterAction : BaseAction
    {
        public int Head { get; set; }
        public int Order { get; set; }
        public int RotationSpeed { get; set; }
        public double Duration { get; set; }
    }
}
