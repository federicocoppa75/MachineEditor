using System.Collections.Generic;
using MachineSteps.Models.Actions;
using MachineSteps.Models.Enums;
using MachineSteps.Models.Steps;
using MachineSteps.Plugins.IsoParser.Attributes;
using MachineSteps.Plugins.IsoIstructions;
using MachineSteps.Plugins.IsoConverterBase;

namespace MachineSteps.Plugins.IsoParser.Converters.M
{
    [MIstructionConverter(161)]
    public class M161Converter : MIstructionConverter<State>
    {
        public override List<MachineStep> Convert(MIstruction istruction, State state)
        {
            return new List<MachineStep>
            {
                new MachineStep()
                {
                    Id = GetStepId(),
                    Name = "M161",
                    Actions = new List<BaseAction>()
                    {
                        new TwoPositionLinkAction()
                        {
                            LinkId = 51,
                            RequestedState = TwoPositionLinkActionRequestedState.Off
                        },
                        new TwoPositionLinkAction()
                        {
                            LinkId = 52,
                            RequestedState = TwoPositionLinkActionRequestedState.Off
                        },
                        new TwoPositionLinkAction()
                        {
                            LinkId = 53,
                            RequestedState = TwoPositionLinkActionRequestedState.Off
                        }
                    }
                }
            };
        }
    }
}
