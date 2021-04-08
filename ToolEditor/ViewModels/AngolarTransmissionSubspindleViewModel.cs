using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace ToolEditor.ViewModels
{
    public class AngolarTransmissionSubspindleViewModel : ViewModelBase
    {
        public string ToolName { get; set; }
        public Point3D Position { get; set; }
        public Vector3D Direction { get; set; }
    }
}
