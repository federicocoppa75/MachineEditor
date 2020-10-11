using g3;
using MachineViewer.Plugins.Panel.MaterialRemoval.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace MachineViewer.Plugins.Panel.MaterialRemoval.Models
{
    public abstract class ImplicitToolBase : BoundedImplicitFunction3d, IAlongDirectionSemplidicable
    {
        protected double _radius;
        protected double _length;
        protected Vector3d _position;

        public ImplicitToolBase(Vector3d position, double length, double radius)
        {
            _position = position;
            _length = length;
            _radius = radius;
        }

        public abstract AxisAlignedBox3d Bounds();
        public abstract double Value(ref Vector3d pt);

        protected abstract bool IsComparable(ImplicitToolBase tool);
        protected abstract int CheckParallel(Vector3d v);

        public AlongDirectionSemplificationCheckResult Check(ImplicitToolBase tool)
        {
            var result = AlongDirectionSemplificationCheckResult.None;

            if(IsComparable(tool))
            {
                var d = tool._position - _position;
                var p = CheckParallel(d);
                
                if (p > 0) result = AlongDirectionSemplificationCheckResult.GoOn;
                else if (p < 0) result = AlongDirectionSemplificationCheckResult.BackOff;
            }

            return result;
        }
    }
}
