using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineViewer.Plugins.Links.SimpleManipolator.Messages
{
    public class LinearPositionGantryOnMessage : LinearPositionGantryBaseMessage
    {
        public override void Execute()
        {
            if(!UnhookedSlave) Master.SetGantrySlave(Slave);
        }
    }
}
