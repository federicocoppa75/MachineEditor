using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

namespace MachineEditor.ViewModels.Colliders
{
    public class ThreePointsColliderViewModel : TwoPointsColliderViewModel
    {
        private Point3D _position3;
        [ExpandableObject]
        public Point3D Position3
        {
            get => _position3;
            set => Set(ref _position3, value, nameof(Position3));
        }
    }
}
