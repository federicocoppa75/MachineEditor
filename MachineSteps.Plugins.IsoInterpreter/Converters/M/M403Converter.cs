using System.Collections.Generic;
using MachineSteps.Models.Actions;
using MachineSteps.Models.Enums;
using MachineSteps.Models.Steps;
using MachineSteps.Plugins.IsoConverterBase;
using MachineSteps.Plugins.IsoInterpreter.Attributes;
using MachineSteps.Plugins.IsoIstructions;

namespace MachineSteps.Plugins.IsoInterpreter.Converters.M
{
    [MIstructionConverter(403)]
    public class M403Converter : MIstructionConverter<State>
    {
        public override List<MachineStep> Convert(MIstruction istruction, State state)
        {
            if (state.Clamp.State == Enums.ClampState.Open)
            {
                state.Clamp.State = Enums.ClampState.Close;

                return new List<MachineStep>
                {
                    new MachineStep()
                    {
                        Id = GetStepId(),
                        Name = "M403",
                        Actions = new List<BaseAction>()
                        {
                            new TwoPositionLinkAction()
                            {
                                LinkId = 11,
                                RequestedState = TwoPositionLinkActionRequestedState.On
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
