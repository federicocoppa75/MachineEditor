using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineSteps.Plugins.IsoParser.Converters.StateData
{
    public class ToolChange
    {
        public class ToolChangeStep
        {
            public int ToolId { get; set; }
            public int SourcePosition { get; set; }
            public int SinkPosition { get; set; }
        }

        public int StartIndex { get; set; }

        public int NumSteps { get; set; }

        public ToolChangeStep[] ToolChangeSteps { get; set; } = new ToolChangeStep[] { new ToolChangeStep(), new ToolChangeStep() };
    }
}
