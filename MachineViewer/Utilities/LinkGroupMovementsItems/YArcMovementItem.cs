using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineViewer.Utilities.LinkGroupMovementsItems
{
    public class YArcMovementItem : ArcMovementItem
    {
        public YArcMovementItem(int linkId, double targetValue) : base(linkId, targetValue)
        {
        }

        public override void SetValue(double k)
        {
            double a = Normalize(StartAngle + Angle * k);

            ActualValue = CenterCoordinate + Math.Sin(a) * Radius;
        }
    }
}
