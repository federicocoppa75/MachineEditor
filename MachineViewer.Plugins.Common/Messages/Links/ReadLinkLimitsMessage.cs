using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineViewer.Plugins.Common.Messages.Links
{
    public class ReadLinkLimitsMessage
    {
        public int LinkId { get; set; }
        public Action<double, double> SetLimits { get; set; }
    }
}
