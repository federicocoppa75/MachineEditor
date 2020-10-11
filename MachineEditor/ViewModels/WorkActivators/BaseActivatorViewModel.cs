using GalaSoft.MvvmLight;
using MachineModels.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

namespace MachineEditor.ViewModels.WorkActivators
{
    public class BaseActivatorViewModel : ViewModelBase
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        private Point3D _positon;
        [ExpandableObject]
        public Point3D Position
        {
            get { return _positon; }
            set { Set(ref _positon, value, nameof(Position)); }
        }

        private Vector3D _direction;
        [ExpandableObject]
        public Vector3D Direction
        {
            get { return _direction; }
            set { Set(ref _direction, value, nameof(Direction)); }
        }
    }
}
