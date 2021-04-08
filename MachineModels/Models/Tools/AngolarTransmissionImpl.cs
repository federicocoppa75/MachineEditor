using MachineModels.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace MachineModels.Models.Tools
{
    public class AngolarTransmissionImpl : Tool
    {
        public class Subspindle
        {
            public Tool Tool { get; set; }
            public Vector Position { get; set; }
            public Vector Direction { get; set; }
        }

        public string BodyModelFile { get; set; }
        public List<Subspindle> Subspindles { get; set; } = new List<Subspindle>();

        public override double GetTotalDiameter() => -1.0;
        public override double GetTotalLength() => -1.0;
        public override ToolType ToolType => ToolType.AngularTransmissionImpl;
    }
}
