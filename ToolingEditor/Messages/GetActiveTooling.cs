using MachineModels.Models.Tooling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToolingEditor.Messages
{
    public class GetActiveTooling
    {
        public Action<Tooling> Set { get; set; }
    }
}
