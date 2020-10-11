using System.Collections.Generic;
using MachineSteps.Models.Actions;
using MachineSteps.Models.Steps;
using MachineSteps.Plugins.IsoParser.Attributes;
using MachineSteps.Plugins.IsoIstructions;
using MachineSteps.Plugins.IsoConverterBase;

namespace MachineSteps.Plugins.IsoParser.Converters.M
{
    [MIstructionConverter(5)]
    public class M5Converter : MIstructionConverter<State>
    {
        public override List<MachineStep> Convert(MIstruction istruction, State state)
        {
            int rotationSpeed = state.RotationSpeed;

            state.RotationSpeed = 0;

            if(state.SelectedHead != 0)
            {
                return new List<MachineStep>()
                {
                    new MachineStep()
                    {
                        Id = GetStepId(),
                        Name = "Turn off spindle",
                        Actions = new List<Models.Actions.BaseAction>()
                        {
                            new TurnOffInverterAction()
                            {
                                Head = state.SelectedHeadTurnedOn,
                                Order = state.HeadOrderTurnedOn,
                                RotationSpeed = rotationSpeed,
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
