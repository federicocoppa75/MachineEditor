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
    public class TwoPositionLinkLazyAction : TwoPositionLinkAction, ILazyAction
    {
        public bool IsUpdated { get; private set; }

        public void Update()
        {
            Messenger.Default.Send(new ReadTwoPositionLinkStateMessage(LinkId, (v) => Update(v)));
        }

        private void Update(bool value)
        {
            RequestedState = value ? MachineSteps.Models.Enums.TwoPositionLinkActionRequestedState.On : MachineSteps.Models.Enums.TwoPositionLinkActionRequestedState.Off;
            IsUpdated = true;
        }
    }
}
