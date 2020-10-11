using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace MachineViewer.Messages.Trace
{
    public class TracePointMessage
    {
        public Point3D Point { get; set; }
        public Brush Brush { get; set; }
    }
}
