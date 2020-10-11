using MachineViewer.Plugins.Panel.MaterialRemoval.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineViewer.Plugins.Panel.MaterialRemoval.Messages
{
    public class SectionToolMoveMessage
    {
        public int XSectionIndex { get; set; }
        public int YSectionIndex { get; set; }
        public ImplicitToolBase Tool { get; set; }
    }
}
