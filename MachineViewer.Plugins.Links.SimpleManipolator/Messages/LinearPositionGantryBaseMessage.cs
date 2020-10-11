
using MachineViewer.Plugins.Common.Messages.Generic;
using MachineViewer.Plugins.Common.Messages.Links;
using MachineViewModels.ViewModels.Links;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineViewer.Plugins.Links.SimpleManipolator.Messages
{
    public abstract class LinearPositionGantryBaseMessage : BaseBackNotificationIdMessage
    {
        public int MasterId { get; set; }
        public int SlaveId { get; set; }
        public ILinearLinkGantryMaster Master { get; set; }
        public IUpdatableValueLink<double> Slave { get; set; }
        public bool IsReady => (Master != null) && ((Slave != null) || VirtualSlave || UnhookedSlave);
        public bool VirtualSlave { get; set; }
        public bool UnhookedSlave { get; set; }

        public abstract void Execute();
    }
}
