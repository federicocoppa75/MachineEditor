using MachineSteps.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineSteps.Models.Actions
{
    [Serializable]
    public class ArcInterpolatedPositionLinkAction : BaseAction, IGradualLinkAction
    {
        public enum ArcDirection
        {
            CW,
            CCW
        }

        [Serializable]
        public class ArcComponent
        {
            public enum ArcComponentType
            {
                X,
                Y
            };

            public int LinkId { get; set; }
            public ArcComponentType Type { get; set; }
            public double CenterCoordinate { get; set; }
            public double TargetCoordinate { get; set; }
        }

        public ArcDirection Direction { get; set; }
        public double Duration { get; set; }
        public double Radius { get; set; }
        public double StartAngle { get; set; }
        public double EndAngle { get; set; }
        public double Angle { get; set; }
        public List<ArcComponent> Components { get; set; }
    }
}
