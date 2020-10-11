using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineSteps.Plugins.IsoInterpreter.Messages.Conversion
{
    public class GIstructionMessage : BaseIstructionMessage
    {
        public int Istruction { get; set; }
        
        public Dictionary<string, double> Coordinates { get; set; }
    }
}
