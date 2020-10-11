using MachineViewer.Plugins.Common.Models.Links.Interpolation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineViewer.Plugins.Common.Messages.Links
{
    public class ArcInterpolationLinkMessage : MoveLinearLinkMessage
    {
        public ArcComponentData ArcComponentData { get; private set; }

        public ArcInterpolationLinkMessage(int id, double targetCoordinate, double duration, ArcComponentData arcComponentData) : base(id, targetCoordinate, duration)
        {
            ArcComponentData = arcComponentData;
        }
    }
}
