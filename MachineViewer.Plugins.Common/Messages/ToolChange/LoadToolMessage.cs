using MachineViewer.Plugins.Common.Messages.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineViewer.Plugins.Common.Messages.ToolChange
{
    public class LoadToolMessage : BaseBackNotificationIdMessage
    {
        public int ToolSource { get; set; }
        public int ToolSink { get; set; }
    }
}
