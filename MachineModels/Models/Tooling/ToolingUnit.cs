using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineModels.Models.Tooling
{
    [Serializable]
    public class ToolingUnit
    {
        public int ToolHolderId { get; set; }
        public string ToolName { get; set; }
    }
}
