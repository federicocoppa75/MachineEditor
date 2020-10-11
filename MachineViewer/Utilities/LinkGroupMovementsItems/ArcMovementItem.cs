using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineViewer.Utilities.LinkGroupMovementsItems
{
    public abstract class ArcMovementItem : MovementItem
    {
        public double StartAngle { get; set; }
        public double Angle { get; set; }
        public double CenterCoordinate { get; set; }
        public double Radius { get; set; }

        public ArcMovementItem(int linkId, double targetValue) : base(linkId, targetValue)
        {
        }

        protected double Normalize(double angle)
        {
            double result = angle;

            if (result > Math.PI) result -= Math.PI * 2.0;
            else if (result < (-Math.PI)) result += Math.PI * 2.0;

            return result;
        }
    }
}
