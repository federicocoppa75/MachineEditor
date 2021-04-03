using MachineModels.Models;
using System.Windows.Media.Media3D;

namespace MachineViewModelUtils.Extensions
{
    public static class Vector3DExtension
    {
        public static Vector3D ToVector3D(this Vector v) => (v != null) ? new Vector3D(v.X, v.Y, v.Z) : new Vector3D();

        public static Vector ToVector(this Vector3D p) => new Vector() { X = p.X, Y = p.Y, Z = p.Z };
    }
}
