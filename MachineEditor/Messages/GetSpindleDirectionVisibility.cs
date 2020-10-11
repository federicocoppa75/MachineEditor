using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineEditor.Messages
{
    public class GetSpindleDirectionVisibility
    {
        public Action<bool> GetVisibility { get; set; }
    }
}
