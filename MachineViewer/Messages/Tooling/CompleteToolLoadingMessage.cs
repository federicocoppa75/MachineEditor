using MachineModels.Models.Tools;
using MachineViewer.Plugins.Common.Messages.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace MachineViewer.Messages.Tooling
{
    public class CompleteToolLoadingMessage : BaseBackNotificationIdMessage
    {
        public int ToolSink { get; set; }
        public Visual3D ToolModel { get; set; }
        public Tool ToolData { get; set; }
    }
}
