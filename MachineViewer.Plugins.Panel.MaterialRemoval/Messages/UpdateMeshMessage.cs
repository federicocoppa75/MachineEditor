using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace MachineViewer.Plugins.Panel.MaterialRemoval.Messages
{
    public class UpdateMeshMessage
    {
        public MeshGeometry3D Mesh { get; set; }
    }
}
