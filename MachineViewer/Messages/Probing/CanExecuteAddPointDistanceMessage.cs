using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineViewer.Messages.Probing
{
    public class CanExecuteAddPointDistanceMessage
    {
        public Action<bool> SetValue { get; set; }
    }
}
