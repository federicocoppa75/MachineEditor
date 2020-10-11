using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineViewer.Plugins.Links.SimpleManipolator.Messages
{
    public class LinearPositionLinkGantryStateChangedMessage
    {
        public int Id { get; set; }
        public bool IsSlave { get; set; }
    }
}
