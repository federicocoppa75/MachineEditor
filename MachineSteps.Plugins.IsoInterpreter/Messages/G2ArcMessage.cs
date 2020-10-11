using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineSteps.Plugins.IsoInterpreter.Messages
{
    public class G2ArcMessage : GArcMessage
    {
        public G2ArcMessage() : base()
        {
            Type = 2;
        }
    }
}
