using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineViewer.Plugins.Common.Messages.Links
{
    public class MoveLinearLinkMessage : UpdateLinkStateMessage<double>
    {
        public double Duration { get; private set; }

        public MoveLinearLinkMessage(int id, double value, double duration) : base(id, value)
        {
            Duration = duration;
        }
    }
}
