using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineModels.Models.Tools
{
    [Serializable]
    public class ToolSet
    {
        public List<Tool> Tools { get; set; } = new List<Tool>();
    }
}
