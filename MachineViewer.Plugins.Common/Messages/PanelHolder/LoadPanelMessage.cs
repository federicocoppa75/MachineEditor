using MachineViewer.Plugins.Common.Messages.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineViewer.Plugins.Common.Messages.PanelHolder
{
    public class LoadPanelMessage : BaseBackNotificationIdMessage
    {
        public int PanelHolderId { get; set; }
        public double Length { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }
        public Action<bool> NotifyExecution { get; set; }
    }
}
