using System.Collections.Generic;
using MachineSteps.Models.Actions;
using MachineSteps.Models.Enums;
using MachineSteps.Models.Steps;
using MachineSteps.Plugins.IsoParser.Attributes;
using MachineSteps.Plugins.IsoIstructions;
using MachineSteps.Plugins.IsoConverterBase;

namespace MachineSteps.Plugins.IsoParser.Converters.M
{
    [MIstructionConverter(160)]
    public class M160Converter : MTurnOnStopConverter
    {
        public M160Converter() : base()
        {
            Name = "M160";
            LinkId = 51;
            PanelHolder = 0;
            CornerReference = PanelCornerReference.Corner2;
        }
    }
}
