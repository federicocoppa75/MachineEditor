using MachineSteps.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineSteps.Models.Actions
{
    [Serializable]
    public class AddPanelAction : BaseAction
    {
        public int PanelId { get; set; }
        public double XDimension { get; set; }
        public double YDimension { get; set; }
        public double ZDimension { get; set; }
        public PanelCornerReference CornerReference { get; set; }
        public int PanelHolder { get; set; }
    }
}
