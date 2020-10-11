using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineSteps.Models.Actions
{
    [Serializable]
    public class LoadToolAction : BaseAction
    {
        public int ToolSource { get; set; }
        public int ToolSink { get; set; }
    }
}
