using MachineViewer.Plugins.Panel.MaterialRemoval.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace MachineViewer.Plugins.Panel.MaterialRemoval.Models
{
    public interface IAlongDirectionSemplidicable
    {
        AlongDirectionSemplificationCheckResult Check(ImplicitToolBase tool);
    }
}
