using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MachineModels.Enums;

namespace MachineModels.Models.WorkActivator
{
    [Serializable]
    public class AnalogicActivator : BaseActivator
    {
        public override ActivatorType ActivatorType => ActivatorType.Analogic;

        public double LowerLimit { get; set; }

        public double UpperLimit { get; set; }

        public double LowerSpeed { get; set; }

        public double UpperSpeed { get; set; }
    }
}
