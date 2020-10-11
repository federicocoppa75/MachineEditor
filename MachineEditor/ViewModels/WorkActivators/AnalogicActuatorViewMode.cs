using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineEditor.ViewModels.WorkActivators
{
    public class AnalogicActuatorViewMode : BaseActivatorViewModel
    {
        public double LowerLimit { get; set; }

        public double UpperLimit { get; set; }

        public double LowerSpeed { get; set; }

        public double UpperSpeed { get; set; }
    }
}
