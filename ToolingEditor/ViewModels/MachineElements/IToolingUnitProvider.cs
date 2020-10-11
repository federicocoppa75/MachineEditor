using MachineModels.Models.Tooling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToolingEditor.ViewModels.MachineElements
{
    public interface IToolingUnitProvider
    {
        ToolingUnit GetToolingUnit();
        int GetToolHolderId();
    }
}
