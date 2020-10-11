using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace MachineViewer.Plugins.Panel.MaterialRemoval.Messages
{
    public class ToolMoveMessage
    {
        public Point3D Position { get; set; }
        public Vector3D Direction { get; set; }
        public double Length { get; set; }
        public double Radius { get; set; }
    }
}
