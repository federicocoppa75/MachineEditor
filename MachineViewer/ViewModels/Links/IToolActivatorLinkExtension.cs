using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineViewer.ViewModels.Links
{
    public interface IToolActivatorLinkExtension
    {
        bool ToolActivator { get; }
        void RegisterToolActivation(Action<bool> action);
    }
}
