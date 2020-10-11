using System.Collections.Generic;
using MachineSteps.Models.Actions;
using MachineSteps.Models.Enums;
using MachineSteps.Models.Steps;
using MachineSteps.Plugins.IsoConverterBase;
using MachineSteps.Plugins.IsoIstructions;

namespace MachineSteps.Plugins.IsoParser.Converters.M
{

    public abstract class MTwoStateIstructionForUnitBaseConverter : MIstructionConverter<State>
    {
        protected abstract TwoPositionLinkActionRequestedState GetRequestedState(State state);

        protected abstract int GetLinkId(State state);

        protected virtual bool IsUnitSelected(State state) => false;

        public override List<MachineStep> Convert(MIstruction istruction, State state)
        {
            if(IsUnitSelected(state))
            {
                return new List<MachineStep>()
                {
                    new MachineStep()
                    {
                        Id = GetStepId(),
                        Name = $"M{istruction.Istruction}",
                        Actions = new List<BaseAction>()
                        {
                            new TwoPositionLinkAction()
                            {
                                LinkId = GetLinkId(state),
                                RequestedState = GetRequestedState(state)
                            }
                        }
                    }
                };
            }
            else
            {
                return null;
            }
        }
    }
}
