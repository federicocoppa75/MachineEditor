using MachineModels.Enums;
using MachineModels.Models.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToolingEditor.ViewModels.MachineElements
{
    public class ToolElementViewModel : MachineElementViewModel, IToolHolderManagement
    {
        private Tool _tool;
        Action _unload;


        public override string Name
        {
            get => _tool.Name;
            set => throw new InvalidOperationException($"Can not set {nameof(Name)} property in {nameof(ToolElementViewModel)}!");
        }        

        public string Description => _tool.Description;

        public ToolType ToolType => _tool.ToolType;

        public double TotalDiameter => _tool.GetTotalDiameter();

        public double TotalLength => _tool.GetTotalLength();

        public string FullDescruption => "";

        public bool IsToolPresent => true;

        public ToolElementViewModel(Tool tool, Action unload)
        {
            _tool = tool;
            _unload = unload;
        }

        public Tool GetTool() => _tool;

        public void LoadTool(Tool tool) => throw new NotImplementedException();

        public void UnloadTool() => _unload();
    }
}
