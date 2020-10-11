using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MachineModels.Enums;

namespace MachineModels.Models.Tools
{
    public class DiskOnConeTool : DiskTool
    {
        public double PostponemntLength { get; set; }

        public double PostponemntDiameter { get; set; }

        public override ToolType ToolType => ToolType.DiskOnCone;

        public override double GetTotalLength()
        {
            var bt = BodyThickness;
            var tl = base.GetTotalLength();

            return PostponemntLength + bt / 2.0 + tl / 2.0;
        }
    }
}
