using MachineModels.Enums;
using MachineModels.Models.Tooling;
using MachineModels.Models.Tools;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToolingEditor.ViewModels.MachineElements
{
    public class ToolHolderViewModel : MachineElementViewModel, IToolHolderManagement, IToolingUnitProvider
    {
        private ObservableCollection<ToolElementViewModel> _tools = new ObservableCollection<ToolElementViewModel>();

        public int ToolHolderId { get; set; }

        public ToolHolderType ToolHolderType { get; set; }

        public bool IsToolPresent => _tools.Count > 0;

        public override IEnumerable<MachineElementViewModel> Children
        {
            get => _tools;
            set => throw new InvalidOperationException($"Can't add children list to {nameof(ToolHolderViewModel)}!");
        }

        public void LoadTool(Tool tool)
        {
            if (IsToolPresent) UnloadTool();

            _tools.Add(new ToolElementViewModel(tool, () => UnloadTool()) { });
            IsExpanded = true;
        }

        public void UnloadTool() => _tools.Clear();

        public ToolingUnit GetToolingUnit()
        {
            return new ToolingUnit()
            {
                ToolHolderId = ToolHolderId,
                ToolName = IsToolPresent ? _tools[0].Name : string.Empty 
            };
        }

        public int GetToolHolderId() => ToolHolderId;
    }
}
