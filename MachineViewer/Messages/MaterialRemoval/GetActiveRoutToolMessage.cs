using MachineModels.Models.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace MachineViewer.Messages.MaterialRemoval
{
    public class GetActiveRoutToolMessage
    {
        public Action<Point3D, Vector3D, Tool, int> SetData { get; set; }
    }
}
