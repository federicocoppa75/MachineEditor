using MachineModels.Models.Links;
using MachineViewer.Plugins.Common.Messages.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineViewer.Plugins.Common.Messages.Links
{
    public abstract class UpdateLinkStateMessage : BaseBackNotificationIdMessage
    {
        public int LinkId { get; set; }

        public UpdateLinkStateMessage(int id)
        {
            LinkId = id;
        }

        public abstract void Update(IUpdatableValueLink link);
    }

    public class UpdateLinkStateMessage<T> : UpdateLinkStateMessage where T : struct
    {
        public T Value { get; private set; }

        public UpdateLinkStateMessage(int id, T value) : base(id)
        {
            Value = value;
        }

        public override void Update(IUpdatableValueLink link)
        {
            if (link.Id == LinkId)
            {
                if (link is IUpdatableValueLink<T> linkT)
                {
                    linkT.Value = Value;
                }
                else
                {
                    throw new ArgumentException("Invalid argument type!");
                }
            }
        }
    }
}
