using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using g3;

namespace MachineViewer.Plugins.Panel.MaterialRemoval.Models
{
    public class ImplicitToolY : ImplicitAxAlignedTool
    {
        protected override int DirKey => 1;

        public ImplicitToolY(Vector3d position, double length, double radius, bool posDirection) : base(position, length, radius, posDirection ? 1.0 : -1.0)
        {
        }

        public override double Value(ref Vector3d pt)
        {
            double result = 0.0;
            var dist = pt - _position;
            var axProjection = dist.y * _axisComponent;

            if ((axProjection >= 0.0) && (axProjection <= _length))
            {
                result = dist.xz.Length - _radius;
            }
            else
            {
                var distXZ = dist.xz.Length;
                var distY = (axProjection < 0) ? Math.Abs(axProjection) : (axProjection - _length);

                if (distXZ <= _radius)
                {
                    result = distY;
                }
                else
                {
                    var r = distXZ - _radius;
                    result = Math.Sqrt(Math.Pow(distY, 2.0) + Math.Pow(r, 2.0));
                }
            }

            return result;
        }

        protected override void InitBox()
        {
            var center = _position;

            center.y += (_length / 2.0) * _axisComponent;

            _box = new AxisAlignedBox3d(center, _radius, _length / 2.0, _radius);
        }
    }
}
