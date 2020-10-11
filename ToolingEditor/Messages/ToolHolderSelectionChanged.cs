using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToolingEditor.ViewModels.MachineElements;

namespace ToolingEditor.Messages
{
    public class ToolHolderSelectionChanged
    {
        public IToolHolderManagement ToolHolder { get; set; }
    }
}
