using GalaSoft.MvvmLight.Messaging;
using MachineSteps.Models.Actions;
using MachineViewer.Plugins.Common.Messages.Links;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineSteps.Plugins.StepsViewer.Models
{
    public class LinearPositionLinkLazyAction : LinearPositionLinkAction, ILazyAction
    {
        public bool IsUpdated { get; private set; }

        public void Update()
        {
            Messenger.Default.Send(new ReadLinearLinkStateMessage(LinkId, (v) => RequestedPosition = v));
            IsUpdated = true;
        }
    }
}
