using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineViewer.Messages.Probing
{
    class CanExecuteRemoveProbeMessage
    {
        public Action<bool> SetValue { get; set; }
    }
}
