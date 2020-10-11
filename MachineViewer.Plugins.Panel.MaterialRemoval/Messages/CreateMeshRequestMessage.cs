using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace MachineViewer.Plugins.Panel.MaterialRemoval.Messages
{
    public class CreateMeshRequestMessage
    {
        public Action<MeshGeometry3D> GetMesh { get; set; }
        public int VerticesCount { get; set; }
        public int TrianglesCount { get; set; }
    }
}
