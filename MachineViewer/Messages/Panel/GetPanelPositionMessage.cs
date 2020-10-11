using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace MachineViewer.Messages.Panel
{
    public class GetPanelPositionMessage
    {
        public Action<bool, Transform3D, Rect3D?> SetData { get; set; }
    }
}
