using MachineSteps.Models.Actions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineSteps.Editor.Messages
{
    class ActionSelectionChangedMessage
    {
        public BaseAction Action { get; set; }
    }
}
