using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MachineModels.Enums;

namespace MachineModels.Models.Tools
{
    [Serializable]
    public class TwoSectionTool : Tool
    {
        public double Diameter1 { get; set; }

        public double Length1 { get; set; }

        public double Diameter2 { get; set; }

        public double Length2 { get; set; }

        public double UsefulLength { get; set; }

        public override double GetTotalDiameter() => Math.Max(Diameter1, Diameter2);

        public override double GetTotalLength() => Length1 + Length2;

        public override ToolType ToolType => ToolType.TwoSection;
    }
}
