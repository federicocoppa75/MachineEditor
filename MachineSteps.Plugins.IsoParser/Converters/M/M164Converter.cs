using System.Collections.Generic;
using MachineSteps.Models.Actions;
using MachineSteps.Models.Enums;
using MachineSteps.Models.Steps;
using MachineSteps.Plugins.IsoParser.Attributes;
using MachineSteps.Plugins.IsoIstructions;
using MachineSteps.Plugins.IsoConverterBase;

namespace MachineSteps.Plugins.IsoParser.Converters.M
{
    [MIstructionConverter(164)]
    public class M164Converter : MTurnOnStopConverter
    {
        public M164Converter()
        {
            Name = "M164";
            LinkId = 53;
            PanelHolder = 2;
            CornerReference = PanelCornerReference.Corner1;
        }
    }
}
