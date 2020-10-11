using System.Collections.Generic;
using MachineSteps.Models.Actions;
using MachineSteps.Models.Steps;
using MachineSteps.Plugins.IsoConverterBase;
using MachineSteps.Plugins.IsoIstructions;

namespace MachineSteps.Plugins.IsoParser.Converters.M
{
    public class MTurnOnInverterConverter : MIstructionConverter<State>
    {
        public override List<MachineStep> Convert(MIstruction istruction, State state)
        {
            if (state.SelectedHead != 0)
            {
                state.HeadOrderTurnedOn = state.HeadOrder;
                state.SelectedHeadTurnedOn = state.SelectedHead;

                return new List<MachineStep>()
                {
                    new MachineStep()
                    {
                        Id = GetStepId(),
                        Name = "Turn On spindle",
                        Actions = new List<Models.Actions.BaseAction>()
                        {
                            new TurnOnInverterAction()
                            {
                                Head = state.SelectedHead,
                                Order = state.HeadOrder,
                                RotationSpeed = state.RotationSpeed,
                                Duration = 1.0
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
