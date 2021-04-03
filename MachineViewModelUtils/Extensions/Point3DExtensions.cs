using MachineModels.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace MachineViewModelUtils.Extensions
{
    public static class Point3DExtensions
    {
        public static Point3D ToPoint3D(this Vector v) => (v != null) ? new Point3D(v.X, v.Y, v.Z) : new Point3D();

        public static Vector ToVector(this Point3D p) => new Vector() { X = p.X, Y = p.Y, Z = p.Z };
    }
}
