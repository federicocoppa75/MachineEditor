using MachineModels.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineViewer.Plugins.Common.Messages.PanelHolder
{
    public class GetAvailablePanelHoldersMessage
    {
        public Action<int, string, PanelLoadType> AvailableToolHolder { get; set; }
    }
}
