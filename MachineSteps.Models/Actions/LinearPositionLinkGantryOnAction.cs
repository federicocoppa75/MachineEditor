using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineSteps.Models.Actions
{
    [Serializable]
    public class LinearPositionLinkGantryOnAction : BaseAction
    {
        public int MasterId { get; set; }
        public int SlaveId { get; set; }
        public bool SlaveUnhooked { get; set; }
    }
}
