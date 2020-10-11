using MachineViewer.Plugins.Common.Messages.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineViewer.Plugins.Common.Messages.Inverters
{
    public class TurnOffInverterMessage : BaseBackNotificationIdMessage
    {
        public int Head { get; set; }
        public int Order { get; set; }
        public double Duration { get; set; }
    }
}
