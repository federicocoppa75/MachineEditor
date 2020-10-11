using System.Collections.Generic;
using MachineSteps.Models.Actions;
using MachineSteps.Models.Enums;
using MachineSteps.Models.Steps;
using MachineSteps.Plugins.IsoConverterBase;
using MachineSteps.Plugins.IsoInterpreter.Attributes;
using MachineSteps.Plugins.IsoIstructions;

namespace MachineSteps.Plugins.IsoInterpreter.Converters.M
{
    [MIstructionConverter(401)]
    public class M401Converter : MIstructionConverter<State>
    {
        public override List<MachineStep> Convert(MIstruction istruction, State state)
        {
            return new List<MachineStep>
            {
                new MachineStep()
                {
                    Id = GetStepId(),
                    Name = "M401",
                    Actions = new List<BaseAction>()
                    {
                        new TwoPositionLinkAction()
                        {
                            LinkId = 10,
                            RequestedState = TwoPositionLinkActionRequestedState.Off
                        }
                    }
                }
            };
        }
    }
}
