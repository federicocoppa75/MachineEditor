using HelixToolkit.Wpf;
using MachineViewer.Enums;
using MachineViewer.Extensions;
using MachineViewModelUtils.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace MachineViewer.ViewModels.Probing
{
    public class PlaneProbeViewModel : ProbeViewModel
    {
        public Vector3D Normal { get; set; }
        public double SizeX { get; set; }
        public double SizeY { get; set; }

        public override ProbeType ProbeType => ProbeType.Plane;

        public static PlaneProbeViewModel Create(MachineElementViewModel parent, Point3D point, double width = 100.0, double height = 100.0)
        {
            var t = parent.GetChainTansform();
            var p = t.Invert().Transform(point);
            var builder = new MeshBuilder();
            var points = new List<Point3D>()
            {
                p + new Vector3D(-width / 2.0, -height / 2.0, 0.0),
                p + new Vector3D(-width / 2.0, height / 2.0, 0.0),
                p + new Vector3D(width / 2.0, height / 2.0, 0.0),
                p + new Vector3D(width / 2.0, -height / 2.0, 0.0)
            };

            builder.AddPolygon(points);

            return new PlaneProbeViewModel()
            {
                X = p.X,
                Y = p.Y,
                Z = p.Z,
                SizeX = width,
                SizeY = height,
                Parent = parent,
                MeshGeometry = builder.ToMesh(),
                Fill = Brushes.Yellow
            };
        }
    }
}
