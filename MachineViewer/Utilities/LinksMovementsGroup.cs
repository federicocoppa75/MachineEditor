using MachineViewer.Plugins.Common.Models.Links.Interpolation;
using MachineViewer.Utilities.LinkGroupMovementsItems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineViewer.Utilities
{
    public class LinksMovementsGroup
    {
        public int GroupId { get; private set; }

        public TimeSpan Duration { get; private set; }

        public DateTime Start { get; private set; }

        public bool IsCompleted { get; private set; }

        public List<MovementItem> Items { get; private set; } = new List<MovementItem>();

        public LinksMovementsGroup(int groupId, double duration)
        {
            GroupId = groupId;
            Duration = TimeSpan.FromSeconds(duration * 2.0);
            Start = DateTime.Now;
        }

        public void Add(int linkId, double value, double targetValue) => Items.Add(new LinearMovementItem(linkId, value, targetValue));

        internal void Add(int linkId, double targetValue, ArcComponentData data)
        {
            ArcMovementItem md = null;

            switch (data.Component)
            {
                case Plugins.Common.Enums.Links.Interpolation.ArcComponent.X:
                    md = new XArcMovementItem(linkId, targetValue);
                    break;
                case Plugins.Common.Enums.Links.Interpolation.ArcComponent.Y:
                    md = new YArcMovementItem(linkId, targetValue);
                    break;
                default:
                    break;
            }

            if(md != null)
            {
                md.Angle = data.Angle;
                md.StartAngle = data.StartAngle;
                md.CenterCoordinate = data.CenterCoordinate;
                md.Radius = data.Radius;

                Items.Add(md);
            }
            else
            {
                throw new ArgumentException("Invalid arc component!");
            }
        }

        public bool Progress(DateTime now)
        {
            bool result = false;
            var elapsed = now - Start;

            if (!IsCompleted)
            {
                if (elapsed >= Duration)
                {
                    Items.ForEach((i) => i.SetTargetValue());
                    IsCompleted = true;
                    result = true;
                }
                else
                {
                    var k = (double)elapsed.TotalMilliseconds / (double)Duration.TotalMilliseconds;

                    Items.ForEach((i) => i.SetValue(k));
                }
            }

            return result;
        }
    }
}
