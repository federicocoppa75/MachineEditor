using MachineSteps.Models.Steps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineSteps.Editor.Messages
{
    class MachineStepSelectionChangedMessage
    {
        public MachineStep MachineStep { get; set; }
    }
}
