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
    public class ArcInterpolatedPositionLinkLazyAction : ArcInterpolatedPositionLinkAction, ILazyAction
    {
        public bool IsUpdated { get; private set; }

        public void Update()
        {
            foreach (var item in Components)
            {
                var i = item;
                Messenger.Default.Send(new ReadLinearLinkStateMessage(i.LinkId, (v) => i.TargetCoordinate = v));
            }

            IsUpdated = true;
        }
    }
}
