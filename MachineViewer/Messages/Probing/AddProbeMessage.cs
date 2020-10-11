using MachineViewer.ViewModels.Probing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineViewer.Messages.Probing
{
    public class AddProbeMessage
    {
        public ProbeViewModel Probe { get; set; }
    }
}
