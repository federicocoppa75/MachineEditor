using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineSteps.Plugins.StepsViewer.Messages
{
    public class WaitForChannelFreeMessage
    {
        public int Channel { get; set; }
        public int BackNotifyId { get; set; }
    }
}
