using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MachineModels.Models;

namespace MachineEditor.Extensions
{
    public static class Matrix3DExtensions
    {
        public static void SumRotation(this Matrix3D m1, Matrix3D m2)
        {
            var r1 = new RotMatrix(m1);
            var r2 = new RotMatrix(m2);

            (r2 * r1).SetRotation(m1);
        }

        public static void SubRotation(this Matrix3D m1, Matrix3D m2)
        {
            var r1 = new RotMatrix(m1);
            var r2 = new RotMatrix(m2);
            var r = r1 * r2.Invert();

            r.SetRotation(m1);
        }

        public static void SumRotation(this Matrix3D m1, ref double x, ref double y, ref double z)
        {
            var r1 = new RotMatrix(m1);

            r1.Trasform(ref x, ref y, ref z);
        }

        public static void SubRotation(this Matrix3D m1, ref double x, ref double y, ref double z)
        {
            var r1 = new RotMatrix(m1).Invert();

            r1.Trasform(ref x, ref y, ref z);
        }
        
        /// <summary>
        /// Converte la matrice di trasformazine ad assoluta (rispetto ad ua precedente trasformazione).
        /// </summary>
        /// <param name="m">matrice da trasformare</param>
        /// <param name="mt">matrice di trasformazione da applicare</param>
        public static void ToAbsoluteReference(this Matrix3D m, Matrix3D mt)
        {
            double x = m.OffsetX;
            double y = m.OffsetY;
            double z = m.OffsetZ;

            mt.SubRotation(ref x, ref y, ref z);

            m.OffsetX = x + mt.OffsetX;
            m.OffsetY = y + mt.OffsetY;
            m.OffsetZ = z + mt.OffsetZ;

            m.SumRotation(mt);
        }

        /// <summary>
        /// Converte la matrice di trasformazine a relativa (rispetto ad ua precedente trasformazione).
        /// </summary>
        /// <param name="m">matrice da trasformare</param>
        /// <param name="mt">matrice di trasformazione da applicare (rispetto alla quale rendere relativa)</param>
        public static void ToRelativeReference(this Matrix3D m, Matrix3D mt)
        {
            double x = m.OffsetX - mt.OffsetX;
            double y = m.OffsetY - mt.OffsetY;
            double z = m.OffsetZ - mt.OffsetZ;

            mt.SumRotation(ref x, ref y, ref z);

            m.OffsetX = x;
            m.OffsetY = y;
            m.OffsetZ = z;

            m.SubRotation(mt);
        }
    }
}
