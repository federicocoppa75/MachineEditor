using MachineSteps.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineSteps.Models.Actions
{
    [Serializable]
    public class TwoPositionLinkAction : LinkAction
    {
        public TwoPositionLinkActionRequestedState RequestedState { get; set; }
    }
}
