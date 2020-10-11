using MachineModels.Models.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineModels.Models.Tooling
{
    [Serializable]
    public class Tooling
    {
        public string MachineFile { get; set; }
        public string ToolsFile { get; set; }
        public List<ToolingUnit> Units { get; set; }
    }
}
