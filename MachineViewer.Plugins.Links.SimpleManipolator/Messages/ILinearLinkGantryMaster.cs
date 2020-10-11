using MachineViewer.Plugins.Common.Messages.Links;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineViewer.Plugins.Links.SimpleManipolator.Messages
{
    public interface ILinearLinkGantryMaster
    {
        void SetGantrySlave(IUpdatableValueLink<double> slave);
        void ResetGantry();
    }
}
