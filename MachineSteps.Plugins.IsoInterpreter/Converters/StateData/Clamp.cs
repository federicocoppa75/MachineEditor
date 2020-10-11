using MachineSteps.Plugins.IsoInterpreter.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineSteps.Plugins.IsoInterpreter.Converters.StateData
{
    public class Clamp
    {
        public ClampPressionState PressionState { get; set; }
        public ClampState State { get; set; }
        public double Distance { get; set; }
    }
}
