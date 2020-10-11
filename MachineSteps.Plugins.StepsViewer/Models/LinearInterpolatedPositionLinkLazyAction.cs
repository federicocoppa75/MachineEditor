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
    public class LinearInterpolatedPositionLinkLazyAction : LinearInterpolatedPositionLinkAction, ILazyAction
    {
        public bool IsUpdated { get; private set; }

        public void Update()
        {
            foreach (var pos in Positions)
            {
                var p = pos;
                Messenger.Default.Send(new ReadLinearLinkStateMessage(p.LinkId, (v) => p.RequestPosition = v));
            }

            IsUpdated = true;
        }
    }
}
