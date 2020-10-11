using MachineViewer.Plugins.Panel.SimpleManipolator.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineViewer.Plugins.Panel.SimpleManipolator.Messages
{
    public class GetPanelDataMessage
    {
        public Action<PanelData> SetPanelData { get; set; }
    }
}
