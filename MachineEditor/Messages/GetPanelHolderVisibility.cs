using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineEditor.Messages
{
    public class GetPanelHolderVisibility
    {
        public Action<bool> GetVisibility { get; set; }
    }
}
