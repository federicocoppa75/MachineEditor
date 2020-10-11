using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

namespace MachineEditor.ViewModels.Colliders
{
    public class EightPointsColliderViewModel : SixPointsColliderViewModel
    {
        private Point3D _positcion7;
        [ExpandableObject]
        public Point3D Position7
        {
            get => _positcion7;
            set => Set(ref _positcion7, value, nameof(Position7));
        }
        private Point3D _position8;
        [ExpandableObject]
        public Point3D Position8
        {
            get => _position8;
            set => Set(ref _position8, value, nameof(Position8));
        }
    }
}
