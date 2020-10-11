using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using g3;

namespace MachineViewer.Plugins.Panel.MaterialRemoval.Models
{
    public class ImplicitToolX : ImplicitAxAlignedTool
    {
        protected override int DirKey => 0;

        public ImplicitToolX(Vector3d position, double length, double radius, bool posDirection) : base(position, length, radius, posDirection ? 1.0 : -1.0)
        {
        }

        public override double Value(ref Vector3d pt)
        {
            double result = 0.0;
            var dist = pt - _position;
            var axProjection = dist.x * _axisComponent;

            if ((axProjection >= 0.0) && (axProjection <= _length))
            {
                result = dist.yz.Length - _radius;
            }
            else
            {
                var distYZ = dist.yz.Length;
                var distX = (axProjection < 0) ? Math.Abs(axProjection) : (axProjection - _length);

                if (distYZ <= _radius)
                {
                    result = distX;
                }
                else
                {
                    var r = distYZ - _radius;
                    result = Math.Sqrt(Math.Pow(distX, 2.0) + Math.Pow(r, 2.0));
                }
            }

            return result;
        }

        protected override void InitBox()
        {
            var center = _position;

            center.x += (_length / 2.0) * _axisComponent;

            _box = new AxisAlignedBox3d(center, _length / 2.0, _radius, _radius);
        }
    }
}
