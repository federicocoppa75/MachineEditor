using MachineModels.Models.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToolEditor.Messages
{
    public class GetToolMessage
    {
        public string Name { get; set; }
        public Action<Tool> SetTool { get; set; }
    }
}
