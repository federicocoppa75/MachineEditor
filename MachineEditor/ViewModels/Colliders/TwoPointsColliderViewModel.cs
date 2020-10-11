using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

namespace MachineEditor.ViewModels.Colliders
{
    public class TwoPointsColliderViewModel : PointColliderViewModel
    {
        private Point3D _position2;
        [ExpandableObject]
        public Point3D Position2
        {
            get => _position2;
            set => Set(ref _position2, value, nameof(Position2));
        }
    }
}
