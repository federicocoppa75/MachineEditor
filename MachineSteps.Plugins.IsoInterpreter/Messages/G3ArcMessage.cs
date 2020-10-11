using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineSteps.Plugins.IsoInterpreter.Messages
{
    public class G3ArcMessage : GArcMessage
    {
        public G3ArcMessage() : base()
        {
            Type = 3;
        }
    }
}
