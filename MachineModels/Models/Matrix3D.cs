using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineModels.Models
{
    [Serializable]
    public class Matrix3D
    {
        public double M11 { get; set; }
        public double M12 { get; set; }
        public double M13 { get; set; }
        public double M14 { get; set; }
        public double M21 { get; set; }
        public double M23 { get; set; }
        public double M24 { get; set; }
        public double M31 { get; set; }
        public double M32 { get; set; }
        public double M33 { get; set; }
        public double M34 { get; set; }
        public double OffsetX { get; set; }
        public double OffsetY { get; set; }
        public double M22 { get; set; }
        public double OffsetZ { get; set; }
        public double M44 { get; set; }
    }

    public class RotMatrix
    {

        private double[,] _values = new double[3, 3];
        public double[,] Values => _values;

        public RotMatrix()
        {
        }

        public RotMatrix(RotMatrix m)
        {
            _values[0, 0] = m._values[0, 0];
            _values[0, 1] = m._values[0, 1];
            _values[0, 2] = m._values[0, 2];
            _values[1, 0] = m._values[1, 0];
            _values[1, 1] = m._values[1, 1];
            _values[1, 2] = m._values[1, 2];
            _values[2, 0] = m._values[2, 0];
            _values[2, 1] = m._values[2, 1];
            _values[2, 2] = m._values[2, 2];
        }

        public RotMatrix(Matrix3D m)
        {
            _values[0, 0] = m.M11;
            _values[0, 1] = m.M12;
            _values[0, 2] = m.M13;
            _values[1, 0] = m.M21;
            _values[1, 1] = m.M22;
            _values[1, 2] = m.M23;
            _values[2, 0] = m.M31;
            _values[2, 1] = m.M32;
            _values[2, 2] = m.M33;
        }

        public RotMatrix Invert()
        {
            var m = new RotMatrix(this);

            _values[0, 0] = m._values[0, 0];
            _values[0, 1] = m._values[1, 0];
            _values[0, 2] = m._values[2, 0];
            _values[1, 0] = m._values[0, 1];
            _values[1, 1] = m._values[1, 1];
            _values[1, 2] = m._values[2, 1];
            _values[2, 0] = m._values[0, 2];
            _values[2, 1] = m._values[1, 2];
            _values[2, 2] = m._values[2, 2];

            return this;
        }

        public void SetRotation(Matrix3D m)
        {
            m.M11 = _values[0, 0];
            m.M12 = _values[0, 1];
            m.M13 = _values[0, 2];
            m.M21 = _values[1, 0];
            m.M22 = _values[1, 1];
            m.M23 = _values[1, 2];
            m.M31 = _values[2, 0];
            m.M32 = _values[2, 1];
            m.M33 = _values[2, 2];
        }

        public static RotMatrix operator *(RotMatrix m1, RotMatrix m2)
        {
            var m = new RotMatrix();

            m.Values[0, 0] = m1.Values[0, 0] * m2.Values[0, 0] + m1.Values[0, 1] * m2.Values[1, 0] + m1.Values[0, 2] * m2.Values[2, 0];
            m.Values[0, 1] = m1.Values[0, 0] * m2.Values[0, 1] + m1.Values[0, 1] * m2.Values[1, 1] + m1.Values[0, 2] * m2.Values[2, 1];
            m.Values[0, 2] = m1.Values[0, 0] * m2.Values[0, 2] + m1.Values[0, 1] * m2.Values[1, 2] + m1.Values[0, 2] * m2.Values[2, 2];

            m.Values[1, 0] = m1.Values[1, 0] * m2.Values[0, 0] + m1.Values[1, 1] * m2.Values[1, 0] + m1.Values[1, 2] * m2.Values[2, 0];
            m.Values[1, 1] = m1.Values[1, 0] * m2.Values[0, 1] + m1.Values[1, 1] * m2.Values[1, 1] + m1.Values[1, 2] * m2.Values[2, 1];
            m.Values[1, 2] = m1.Values[1, 0] * m2.Values[0, 2] + m1.Values[1, 1] * m2.Values[1, 2] + m1.Values[1, 2] * m2.Values[2, 2];

            m.Values[2, 0] = m1.Values[2, 0] * m2.Values[0, 0] + m1.Values[2, 1] * m2.Values[1, 0] + m1.Values[2, 2] * m2.Values[2, 0];
            m.Values[2, 1] = m1.Values[2, 0] * m2.Values[0, 1] + m1.Values[2, 1] * m2.Values[1, 1] + m1.Values[2, 2] * m2.Values[2, 1];
            m.Values[2, 2] = m1.Values[2, 0] * m2.Values[0, 2] + m1.Values[2, 1] * m2.Values[1, 2] + m1.Values[2, 2] * m2.Values[2, 2];

            return m;
        }

        public void Trasform(ref double x, ref double y, ref double z)
        {
            double xx = Values[0, 0] * x + Values[0, 1] * y + Values[0, 2] * z;
            double yy = Values[1, 0] * x + Values[1, 1] * y + Values[1, 2] * z;
            double zz = Values[2, 0] * x + Values[2, 1] * y + Values[2, 2] * z;

            x = xx;
            y = yy;
            z = zz;
        }
    }
}
