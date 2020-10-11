using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineViewer.Plugins.Common.Messages.Links
{
    public class ReadTwoPositionLinkDurationMessage
    {
        public int LinkId { get; set; }
        public bool RequestedState { get; set; }
        public Action<double> SetDuration { get; set; }
    }
}
