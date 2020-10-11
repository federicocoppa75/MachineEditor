using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace MachineViewer.Messages.Panel
{
    public class GetPanelTransformMessage
    {
        //public Action<bool, Transform3D> SetData { get; set; }
        public Action<bool, Matrix3D> SetData { get; set; }
    }
}
