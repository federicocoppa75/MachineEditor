using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace MachineViewer.ViewModels.Inserter
{
    public abstract class InserterBaseViewModel : MachineElementViewModel
    {
        protected Transform3D _chainTransform;

        public int InserterId { get; set; }

        public Point3D Position { get; set; }

        public Vector3D Direction { get; set; }

        public Color Color { get; set; }

        public InserterBaseViewModel() : base()
        {

        }
    }
}
