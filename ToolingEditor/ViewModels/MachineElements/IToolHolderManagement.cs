using MachineModels.Models.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToolingEditor.ViewModels.MachineElements
{
    public interface IToolHolderManagement
    {
        bool IsToolPresent { get; }

        void LoadTool(Tool tool);

        void UnloadTool();
    }
}
