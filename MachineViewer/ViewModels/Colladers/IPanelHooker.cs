using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace MachineViewer.ViewModels.Colladers
{
    public interface IPanelHooker
    {
        Transform3D TotalTransformation { get; }

        void HookPanel(Visual3D panel);

        Visual3D UnhookPanel();
    }
}
