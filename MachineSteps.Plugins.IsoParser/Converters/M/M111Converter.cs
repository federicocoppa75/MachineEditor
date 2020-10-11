using MachineSteps.Models.Enums;
using MachineSteps.Plugins.IsoParser.Attributes;

namespace MachineSteps.Plugins.IsoParser.Converters.M
{
    [MIstructionConverter(111)]
    public class M111Converter : MTwoStateIstructionForUnitBaseConverter
    {
        protected override bool IsUnitSelected(State state)
        {
            return (state.SelectedHead == 1) && ((state.HeadOrder == 1) || (state.HeadOrder == 2));
        }

        protected override int GetLinkId(State state) => (state.HeadOrder == 1) ? 3101 : 6101;

        protected override TwoPositionLinkActionRequestedState GetRequestedState(State state) => TwoPositionLinkActionRequestedState.On;

    }
}
