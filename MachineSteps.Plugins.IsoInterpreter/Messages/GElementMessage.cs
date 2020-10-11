using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineSteps.Plugins.IsoInterpreter.Messages
{
    public class GElementMessage
    {
        public int Type { get; set; }
        public Dictionary<string, string> Coordinates { get; set; }

        public bool IsIncremental { get; set; }
    }
}
