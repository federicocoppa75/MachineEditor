using MachineModels.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineViewer.ViewModels.ToolHolder
{
    public class StaticToolHolderViewModel : ToolHolderViewModel
    {
        public override ToolHolderType ToolHolderType => ToolHolderType.Static;

        public StaticToolHolderViewModel() : base()
        {
        }
    }
}
