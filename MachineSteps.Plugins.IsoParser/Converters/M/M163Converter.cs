using System.Collections.Generic;
using MachineSteps.Models.Actions;
using MachineSteps.Models.Enums;
using MachineSteps.Models.Steps;
using MachineSteps.Plugins.IsoParser.Attributes;
using MachineSteps.Plugins.IsoIstructions;
using MachineSteps.Plugins.IsoConverterBase;

namespace MachineSteps.Plugins.IsoParser.Converters.M
{
    [MIstructionConverter(163)]
    public class M163Converter : MTurnOnStopConverter
    {
        public M163Converter() : base()
        {
            Name = "M163";
            LinkId = 52;
            PanelHolder = 1;
            CornerReference = PanelCornerReference.Corner1;
        }
    }
}
