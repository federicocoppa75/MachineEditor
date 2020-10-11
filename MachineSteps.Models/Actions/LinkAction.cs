using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineSteps.Models.Actions
{
    [Serializable]
    public abstract class LinkAction : BaseAction, ILinkAction
    {
        public int LinkId { get; set; }
    }
}
