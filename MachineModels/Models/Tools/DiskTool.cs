using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MachineModels.Enums;

namespace MachineModels.Models.Tools
{
    [Serializable]
    public class DiskTool : Tool
    {
        public double Diameter { get; set; }

        public double CuttingRadialThickness { get; set; }

        public double BodyThickness { get; set; }

        public double CuttingThickness { get; set; }

        public double RadialUsefulLength { get; set; }

        public override double GetTotalDiameter() => Diameter;

        public override double GetTotalLength() => Math.Max(CuttingThickness, BodyThickness);

        public override ToolType ToolType => ToolType.Disk;
    }
}
