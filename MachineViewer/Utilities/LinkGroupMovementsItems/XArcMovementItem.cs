using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineViewer.Utilities.LinkGroupMovementsItems
{
    public class XArcMovementItem : ArcMovementItem
    {
        public XArcMovementItem(int linkId, double targetValue) : base(linkId, targetValue)
        {
        }

        public override void SetValue(double k)
        {
            double a = Normalize(StartAngle + Angle * k);

            ActualValue = CenterCoordinate + Math.Cos(a) * Radius;
        }
    }
}
