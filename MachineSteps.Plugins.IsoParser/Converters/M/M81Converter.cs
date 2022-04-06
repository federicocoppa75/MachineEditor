using MachineSteps.Models.Actions;
using MachineSteps.Models.Enums;
using MachineSteps.Models.Steps;
using MachineSteps.Plugins.IsoIstructions;
using MachineSteps.Plugins.IsoParser.Attributes;
using System.Collections.Generic;

namespace MachineSteps.Plugins.IsoParser.Converters.M
{
    [MIstructionConverter(81)]
    public class M81Converter : MTwoStateIstructionForUnitBaseConverter
    {
        protected override bool IsUnitSelected(State state)
        {
            return (state.SelectedHead == 1) && ((state.HeadOrder == 1) || (state.HeadOrder == 2));
        }

        protected override int GetLinkId(State state) => (state.HeadOrder == 1) ? 3001 : 6001;

        protected override TwoPositionLinkActionRequestedState GetRequestedState(State state) => TwoPositionLinkActionRequestedState.Off;

        public override List<MachineStep> Convert(MIstruction istruction, State state)
        {
            var list = base.Convert(istruction, state);

            if ((list != null) &&
                (list.Count == 1) &&
                (GetLinkId(state) == 3001) &&
                (state.HeadSetup != 0))
            {
                list[0].Actions.Add(new TwoPositionLinkAction()
                {
                    LinkId = 3002,
                    RequestedState = GetRequestedState(state)
                });
            }

            return list;
        }
    }
}
