using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MachineModels.Enums;

namespace MachineModels.Models.Tools
{
    [Serializable]
    public class SimpleTool : Tool
    {
        public double Diameter { get; set; }

        public double Length { get; set; }

        public double UsefulLength { get; set; }

        public override double GetTotalDiameter() => Diameter;

        public override double GetTotalLength() => Length;

        public override ToolType ToolType => ToolType.Simple;
    }
}
