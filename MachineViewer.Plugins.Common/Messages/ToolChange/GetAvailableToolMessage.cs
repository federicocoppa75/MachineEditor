using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineViewer.Plugins.Common.Messages.ToolChange
{
    public class GetAvailableToolMessage
    {
        public Action<int, string> SetAvailableTool { get; set; }
    }
}
