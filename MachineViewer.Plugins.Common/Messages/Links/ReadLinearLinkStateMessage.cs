using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineViewer.Plugins.Common.Messages.Links
{
    public class ReadLinearLinkStateMessage : ReadLinkStateMessage<double>
    {
        public ReadLinearLinkStateMessage(int id, Action<double> setValue) : base(id, setValue)
        {
        }
    }
}
