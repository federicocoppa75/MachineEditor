using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineViewer.Plugins.Common.Messages.Links
{
    public abstract class ReadLinkStateMessage
    {
        public int LinkId { get; set; }

        public ReadLinkStateMessage(int id)
        {
            LinkId = id;
        }

        public abstract void Read(IUpdatableValueLink link);
    }

    public class ReadLinkStateMessage<T> : ReadLinkStateMessage where T : struct
    {
        public Action<T> SetValue { get; private set; }

        public ReadLinkStateMessage(int id, Action<T> setValue) : base(id)
        {
            SetValue = setValue;
        }

        public override void Read(IUpdatableValueLink link)
        {
            if (link.Id == LinkId)
            {
                if (link is IUpdatableValueLink<T> linkT)
                {
                     SetValue?.Invoke(linkT.Value);
                }
                else
                {
                    throw new ArgumentException("Invalid argument type!");
                }
            }
        }
    }
}
