using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineViewer.Plugins.Common.Messages.Links
{
    public class UpdateLinearLinkStateToTargetMessage : UpdateLinearLinkStateMessage
    {
        public bool IsCompleted { get; private set; }

        public UpdateLinearLinkStateToTargetMessage(int id, double value, bool isCompleted = false) : base(id, value)
        {
            IsCompleted = isCompleted;
        }
    }
}
