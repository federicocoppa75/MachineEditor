using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineViewModelUtils.Extensions
{
    public static class ModelElementDataExtension
    {
        public static System.Windows.Media.Color Convert(this MachineModels.Models.Color c)
        {
            return new System.Windows.Media.Color()
            {
                A = c.A,
                R = c.R,
                G = c.G,
                B = c.B
            };
        }

        public static MachineModels.Models.Color Convert(this System.Windows.Media.Color c)
        {
            return new MachineModels.Models.Color()
            {
                A = c.A,
                R = c.R,
                G = c.G,
                B = c.B
            };
        }

        public static System.Windows.Media.Media3D.Matrix3D Convert(this MachineModels.Models.Matrix3D m)
        {
            return new System.Windows.Media.Media3D.Matrix3D()
            {
                M11 = m.M11,
                M12 = m.M12,
                M13 = m.M13,
                M14 = m.M14,
                M21 = m.M21,
                M22 = m.M22,
                M23 = m.M23,
                M24 = m.M24,
                M31 = m.M31,
                M32 = m.M32,
                M33 = m.M33,
                M34 = m.M34,
                OffsetX = m.OffsetX,
                OffsetY = m.OffsetY,
                OffsetZ = m.OffsetZ,
                M44 = m.M44,
            };
        }
    }
}
