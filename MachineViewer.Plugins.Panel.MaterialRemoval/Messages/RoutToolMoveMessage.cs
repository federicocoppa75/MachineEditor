using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineViewer.Plugins.Panel.MaterialRemoval.Messages
{
    public class RoutToolMoveMessage : ToolMoveMessage
    {
        public int ToolId { get; set; }
    }
}
