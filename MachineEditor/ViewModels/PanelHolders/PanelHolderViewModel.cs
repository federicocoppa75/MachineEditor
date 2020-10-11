using GalaSoft.MvvmLight;
using MachineModels.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

namespace MachineEditor.ViewModels.PanelHolders
{
    public class PanelHolderViewModel : ViewModelBase
    {
        public int Id { get; set; }

        public string Name { get; set; }

        private Point3D _position;
        [ExpandableObject]
        public Point3D Position
        {
            get => _position;
            set => Set(ref _position, value, nameof(Position));
        }

        public PanelLoadType Corner { get; set; }
    }
}
