using MachineModels.Enums;
using MachineModels.Models.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToolingEditor.ViewModels.Tools
{
    public class ToolViewModel
    {
        private Tool _tool;

        public string Name => _tool.Name;

        public string Description => _tool.Description;

        public ToolType ToolType => _tool.ToolType;

        public double TotalDiameter => _tool.GetTotalDiameter();

        public double TotalLength => _tool.GetTotalLength();

        public string FullDescruption => "";

        public ToolViewModel(Tool tool)
        {
            _tool = tool;
        }

        public Tool GetModel() => _tool;
    }
}
