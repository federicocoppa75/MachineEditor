using System.Collections.Generic;
using MachineSteps.Models.Actions;
using MachineSteps.Models.Enums;
using MachineSteps.Models.Steps;
using MachineSteps.Plugins.IsoConverterBase;
using MachineSteps.Plugins.IsoInterpreter.Attributes;
using MachineSteps.Plugins.IsoIstructions;

namespace MachineSteps.Plugins.IsoInterpreter.Converters.M
{
    [MIstructionConverter(402)]
    public class M402Converter : MIstructionConverter<State>
    {
        public override List<MachineStep> Convert(MIstruction istruction, State state)
        {
            if(state.Clamp.State == Enums.ClampState.Close)
            {
                state.Clamp.State = Enums.ClampState.Open;

                return new List<MachineStep>
                {                    
                    new MachineStep()
                    {
                        Id = GetStepId(),
                        Name = "M402",
                        Actions = new List<BaseAction>()
                        {
                            new TwoPositionLinkAction()
                            {
                                LinkId = 11,
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
