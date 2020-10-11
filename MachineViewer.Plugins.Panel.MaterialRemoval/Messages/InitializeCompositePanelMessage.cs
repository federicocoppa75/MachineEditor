using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineViewer.Plugins.Panel.MaterialRemoval.Messages
{
    public class InitializeCompositePanelMessage
    {
        public double SizeX { get; set; }
        public double SizeY { get; set; }
        public double SizeZ { get; set; }
    }
}
