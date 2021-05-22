using System;
using System.Collections.Generic;
using System.Text;

namespace MachineSteps.Models.Actions
{
    [Serializable]
    public class ChannelWaiterAction : BaseAction
    {
        public int ChannelToWait { get; set; }
    }
}
