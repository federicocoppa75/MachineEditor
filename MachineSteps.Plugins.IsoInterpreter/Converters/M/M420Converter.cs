using System.Collections.Generic;
using MachineSteps.Models.Actions;
using MachineSteps.Models.Enums;
using MachineSteps.Models.Steps;
using MachineSteps.Plugins.IsoConverterBase;
using MachineSteps.Plugins.IsoInterpreter.Attributes;
using MachineSteps.Plugins.IsoIstructions;

namespace MachineSteps.Plugins.IsoInterpreter.Converters.M
{
    [MIstructionConverter(420)]
    public class M420Converter : MIstructionConverter<State>
    {
        public override List<MachineStep> Convert(MIstruction istruction, State state)
        {
            if (state.LateralPresser.LateralPresserState == Enums.LateralPresserState.On)
            {
                state.LateralPresser.LateralPresserState = Enums.LateralPresserState.Off;

                return new List<MachineStep>
                {
                    new MachineStep()
                    {
                        Id = GetStepId(),
                        Name = "M420",
                        Actions = new List<BaseAction>()
                        {
                            new TwoPositionLinkAction()
                            {
                                LinkId = 20,
                                RequestedState = TwoPositionLinkActionRequestedState.Off
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
