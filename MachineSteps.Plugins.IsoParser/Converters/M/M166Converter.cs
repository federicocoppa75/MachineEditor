using MachineSteps.Models.Enums;
using MachineSteps.Plugins.IsoParser.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineSteps.Plugins.IsoParser.Converters.M
{
    [MIstructionConverter(166)]
    public class M166Converter : MTwoStateIstructionForUnitBaseConverter
    {
        protected override bool IsUnitSelected(State state) => (state.SelectedHead == 1) && (state.HeadOrder == 2);

        protected override int GetLinkId(State state) => 12001;

        protected override TwoPositionLinkActionRequestedState GetRequestedState(State state) => TwoPositionLinkActionRequestedState.Off;
    }
}
