using MachineModels.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace MachineModels.Models.Tools
{
    [Serializable]
    public class AngolarTransmission : Tool
    {
        public string BodyModelFile { get; set; }
        public string ToolName { get; set; }
        public Vector Position { get; set; }
        public Vector Direction { get; set; }

        public override double GetTotalDiameter() => -1.0;

        public override double GetTotalLength() => -1.0;

        public override ToolType ToolType => ToolType.AngularTransmission;
    }
}
