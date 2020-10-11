using MachineModels.Models.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineViewer.Messages.Tooling
{
    public class LoadToolMessage
    {
        public int ToolHolderId { get; set; }
        public Tool Tool { get; set; }
    }
}
