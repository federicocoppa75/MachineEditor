using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MachineModels.Enums;

namespace MachineModels.Models.WorkActivator
{
    [Serializable]
    public class DigitalActivator : BaseActivator
    {
        public override ActivatorType ActivatorType => ActivatorType.Digital;

        public double SwitchOnInterval { get; set; }

        public double SwitchOffInterval { get; set; }

        public double ScwitchOnValue { get; set; }

        public double SwitchOffValue { get; set; }
    }
}
