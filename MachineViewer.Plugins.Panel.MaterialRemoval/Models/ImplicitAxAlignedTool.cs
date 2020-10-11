using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using g3;

namespace MachineViewer.Plugins.Panel.MaterialRemoval.Models
{
    public abstract class ImplicitAxAlignedTool : ImplicitToolBase
    {
        protected AxisAlignedBox3d _box;
        protected double _axisComponent;

        protected abstract int DirKey { get; }

        public ImplicitAxAlignedTool(Vector3d position, double length, double radius, double axisComponent) : base(position, length, radius)
        {
            _axisComponent = axisComponent;
            InitBox();
        }

        protected abstract void InitBox();

        public override AxisAlignedBox3d Bounds() => _box;

        protected override bool IsComparable(ImplicitToolBase tool)
        {
            bool result = false;

            if((tool != null) && (tool is ImplicitAxAlignedTool aaTool))
            {
                if((DirKey == aaTool.DirKey) &&
                   (_axisComponent == aaTool._axisComponent) &&
                   (_radius == aaTool._radius) &&
                   (_length == aaTool._length))
                {
                    result = true;
                }
            }

            return result;
        }

        protected override int CheckParallel(Vector3d v)
        {
            int result = 0;
            int key = DirKey;

            for (int i = 0; i < 3; i++)
            {
                if(i == key)
                {
                    var d = v[i] * _axisComponent;

                    if (d > 0.0) result = 1;
                    else if (d < 0.0) result = -1;
                }
                else
                {
                    if (v[i] != 0.0)
                    {
                        result = 0;
                        break;
                    }
                }
            }

            return result;
        }
    }
}
