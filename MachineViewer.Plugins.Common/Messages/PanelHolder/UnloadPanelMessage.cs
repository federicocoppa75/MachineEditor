using MachineViewer.Plugins.Common.Messages.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineViewer.Plugins.Common.Messages.PanelHolder
{
    public class UnloadPanelMessage : BaseBackNotificationIdMessage
    {
        public int PanelHolderId { get; set; }
        public Action<bool> NotifyExecution { get; set; }
    }
}
