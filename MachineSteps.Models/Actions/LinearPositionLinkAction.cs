using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineSteps.Models.Actions
{
    [Serializable]
    public class LinearPositionLinkAction : LinkAction, IGradualLinkAction
    {
        public double RequestedPosition { get; set; }

        public double Duration { get; set; }
    }
}
