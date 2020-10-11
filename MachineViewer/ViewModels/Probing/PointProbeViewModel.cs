using HelixToolkit.Wpf;
using MachineViewer.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using MachineViewModelUtils.Extensions;
using System.Windows;
using MachineViewer.Enums;

namespace MachineViewer.ViewModels.Probing
{
    public class PointProbeViewModel : ProbeViewModel
    {
        private Action<bool> _onIsSelectedChanged;

        public double Radius { get; set; }

        public override ProbeType ProbeType => ProbeType.Point;

        public static PointProbeViewModel Create(MachineElementViewModel parent, Point3D point, double radius = 5.0)
        {
            var t = parent.GetChainTansform();
            var p = t.Invert().Transform(point);
            var builder = new MeshBuilder();

            builder.AddSphere(p, radius);

            var vm = new PointProbeViewModel()
            {
                X = p.X,
                Y = p.Y,
                Z = p.Z,
                Radius = radius,
                Parent = parent,
                MeshGeometry = builder.ToMesh(),
                Fill = Brushes.Yellow
            };

            vm._onIsSelectedChanged = (b) => vm.Fill = b ? Brushes.Red : Brushes.Yellow;
            vm.PropertyChanged += (s, e) => vm.OnPropertyChanged(s, e);

            return vm;
        }

        private void OnPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (string.Compare(e.PropertyName, "IsSelected") == 0) _onIsSelectedChanged?.Invoke(IsSelected);
        }
    }
}
