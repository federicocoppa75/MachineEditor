using MachineViewer.ViewModels;
using MachineViewer.ViewModels.Colladers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace MachineViewer.Messages.Panel
{
    public class HookPanelMessage
    {
        /// <summary>
        /// elemento macchina a cui agganciare il pannello.
        /// </summary>
        public IPanelHooker Hooker { get; set; }
    }
}
