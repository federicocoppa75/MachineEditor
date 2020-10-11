using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

namespace MachineEditor.ViewModels.Colliders
{
    public class FourPointsColliderViewModel : ThreePointsColliderViewModel
    {
        private Point3D _position4;
        [ExpandableObject]
        public Point3D Position4
        {
            get => _position4;
            set => Set(ref _position4, value, nameof(Position4));
        }
    }
}
