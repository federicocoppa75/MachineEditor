using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

namespace MachineEditor.ViewModels.Colliders
{
    public class SixPointsColliderViewModel : FourPointsColliderViewModel
    {
        private Point3D _positcion5;
        [ExpandableObject]
        public Point3D Position5
        {
            get => _positcion5;
            set => Set(ref _positcion5, value, nameof(Position5));
        }
        private Point3D _position6;
        [ExpandableObject]
        public Point3D Position6
        {
            get => _position6;
            set => Set(ref _position6, value, nameof(Position6));
        }
    }
}
