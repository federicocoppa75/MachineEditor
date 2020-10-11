using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineViewer.Plugins.Common.Messages.Links
{
    public class ReadTwoPositionLinkStateMessage : ReadLinkStateMessage<bool>
    {
        public ReadTwoPositionLinkStateMessage(int id, Action<bool> setValue) : base(id, setValue)
        {
        }
    }
}
