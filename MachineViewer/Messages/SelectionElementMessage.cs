using MachineViewer.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace MachineViewer.Messages
{
    public class SelectionElementMessage
    {        
        public MeshGeometry3D Selected { get; set; }

        public Action<MachineElementViewModel> SetSelected { get; set; }
    }
}
