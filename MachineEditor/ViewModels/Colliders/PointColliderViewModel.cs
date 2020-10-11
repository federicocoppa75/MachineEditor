using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

namespace MachineEditor.ViewModels.Colliders
{
    public class PointColliderViewModel : ColliderViewModel
    {
        private Point3D _position;
        [ExpandableObject]
        public Point3D Position
        {
            get => _position;
            set => Set(ref _position, value, nameof(Position));
        }

        public double Radius { get; set; }
    }
}
