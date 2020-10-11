using g3;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineViewer.Plugins.Panel.MaterialRemoval.Models
{
    public class ImplicitRoutingY : ImplicitAxAlignedRouting
    {
        protected override int DirKey => 1;

        #region ctor

        public ImplicitRoutingY(double length, double radius, int toolId, bool posDirection) : base(length, radius, toolId, posDirection ? 1.0 : -1.0)
        {
        }

        #endregion

        #region overridden

        protected override Vector2d GetPlaneComponent(ref Vector3d v) => v.xz;

        protected override AxisAlignedBox3d ToolPositionBox(ref Vector3d position)
        {
            var center = position;

            center.y += (_length / 2.0) * _axisComponent;

            return new AxisAlignedBox3d(center, _radius, _length / 2.0, _radius);
        }

        #endregion
    }
}
