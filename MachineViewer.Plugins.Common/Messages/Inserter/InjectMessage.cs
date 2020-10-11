using MachineViewer.Plugins.Common.Messages.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineViewer.Plugins.Common.Messages.Inserter
{
    public class InjectMessage : BaseBackNotificationIdMessage
    {
        public int InjectorId { get; set; }

        public double Duration { get; set; }
    }
}
