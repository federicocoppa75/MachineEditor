using MachineSteps.Models.Enums;
using MachineSteps.Plugins.IsoParser.Attributes;

namespace MachineSteps.Plugins.IsoParser.Converters.M
{
    [MIstructionConverter(25)]
    public class M25Converter : MTwoStateIstructionForUnitBaseConverter
    {
        protected override bool IsUnitSelected(State state) => (state.SelectedHead == 1) && (state.HeadOrder == 2);

        protected override int GetLinkId(State state) => 6002;

        protected override TwoPositionLinkActionRequestedState GetRequestedState(State state) => TwoPositionLinkActionRequestedState.Off;
    }
}
