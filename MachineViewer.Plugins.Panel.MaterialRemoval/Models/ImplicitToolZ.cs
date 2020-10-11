using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using g3;

namespace MachineViewer.Plugins.Panel.MaterialRemoval.Models
{
    public class ImplicitToolZ : ImplicitAxAlignedTool
    {
        protected override int DirKey => 2;

        public ImplicitToolZ(Vector3d position, double length, double radius, bool posDirection) : base(position, length, radius, posDirection ? 1.0 : -1.0)
        {
        }

        public override double Value(ref Vector3d pt)
        {
            double result = 0.0;
            var dist = pt - _position;
            var axProjection = dist.z * _axisComponent;

            if((axProjection >= 0.0) && (axProjection <= _length))
            {
                result = dist.xy.Length - _radius;
            }
            else
            {
                var distXY = dist.xy.Length;
                var distZ = (axProjection < 0) ? Math.Abs(axProjection) : (axProjection - _length); 

                if(distXY <= _radius)
                {
                    result = distZ;
                }
                else
                {
                    var r = distXY - _radius;
                    result = Math.Sqrt(Math.Pow(distZ, 2.0) + Math.Pow(r, 2.0));
                }
            }

            return result;
        }

        protected override void InitBox()
        {
            var center = _position;

            center.z += (_length / 2.0) * _axisComponent;

            _box = new AxisAlignedBox3d(center, _radius, _radius, _length / 2.0);
        }
    }
}
