using MachineModels.Models.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToolEditor.Messages
{
    public class SelectedToolChanged<T> where T : Tool
    {
        public T Tool { get; set; }

        public SelectedToolChanged<U> DownCast<U>() where U : T
        {
            var t = Tool as U;

            return new SelectedToolChanged<U>() { Tool = t };
        }
            
    }
}
