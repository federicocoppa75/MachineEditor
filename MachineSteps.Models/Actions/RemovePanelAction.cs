using MachineSteps.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineSteps.Models.Actions
{
    [Serializable]
    public class RemovePanelAction : BaseAction
    {
        public int PanelId { get; set; }
        public PanelCornerReference CornerReference { get; set; }
        public int PanelHolder { get; set; }
    }
}
