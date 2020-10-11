using HelixToolkit.Wpf;
using MachineViewer.Enums;
using MachineViewer.Extensions;
using MachineViewModelUtils.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace MachineViewer.ViewModels.Probing
{
    public class PointsDistanceViewModel : ProbeViewModel
    {
        //private PointProbeViewModel _vm1;
        //private PointProbeViewModel _vm2;
        private Action<bool> _onIsSelectedChanged;

        public override ProbeType ProbeType => ProbeType.Distance;

        public static PointsDistanceViewModel Create(PointProbeViewModel vm1, PointProbeViewModel vm2)
        {
            var p1 = new Point3D(vm1.X, vm1.Y, vm1.Z);
            var p2 = new Point3D(vm2.X, vm2.Y, vm2.Z);
            var t1 = vm1.GetChainTansform();
            var t2 = vm2.GetChainTansform();
            var pp2 = t1.Invert().Transform(t2.Transform(p2));
            var p12 = pp2 - p1;

            var pdvm = new PointsDistanceViewModel()
            {
                X = p12.X,
                Y = p12.Y,
                Z = p12.Z,
                //_vm1 = vm1,
                //_vm2 = vm2,
                Parent = vm1
            };

            var points = new Point3DCollection()
            {
                p1,
                p1 + new Vector3D(p12.X, 0.0, 0.0),
                p1 + new Vector3D(p12.X, 0.0, 0.0),
                p1 + new Vector3D(p12.X, p12.Y, 0.0),
                p1 + new Vector3D(p12.X, p12.Y, 0.0),
                pp2
            };

            var line = new LinesVisual3D()
            {
                Points = points,
                Color = Colors.Yellow,
                Thickness = 3
            };

            pdvm.Children.Add(line);
            pdvm._onIsSelectedChanged = (b) => line.Color = b ? Colors.Red : Colors.Yellow;
            pdvm.PropertyChanged += (s, e) => pdvm.OnPropertyChanged(s, e);
 
            return pdvm;
        }

        private void OnPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if(string.Compare(e.PropertyName, "IsSelected") == 0) _onIsSelectedChanged?.Invoke(IsSelected);
        }
    }
}
