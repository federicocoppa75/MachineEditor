using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineSteps.Models.Actions
{
    [Serializable]
    public class LinearInterpolatedPositionLinkAction : BaseAction, IGradualLinkAction
    {
        [Serializable]
        public class PositionItem
        {
            public int LinkId { get; set; }
            public double RequestPosition { get; set; }
        }

        public List<PositionItem> Positions { get; set; }

        public double Duration { get; set; }
    }
}
