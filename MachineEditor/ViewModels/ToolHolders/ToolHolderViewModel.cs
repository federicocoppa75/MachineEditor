using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

namespace MachineEditor.ViewModels.ToolHolders
{
    public class ToolHolderViewModel : ViewModelBase, IToolHolderViewModel
    {
        public int Id { get; set; }

        private Point3D _position;
        [ExpandableObject]
        public Point3D Position
        {
            get { return _position; }
            set { Set(ref _position, value, nameof(Position)); }
        }

        private Vector3D _direction = new Vector3D(0.0, 0.0, -1.0);
        [ExpandableObject]
        public Vector3D Direction
        {
            get { return _direction; }
            set { Set(ref _direction, value, nameof(Direction)); }
        }
    }
}
