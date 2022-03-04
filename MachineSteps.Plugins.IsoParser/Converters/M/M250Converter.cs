using MachineSteps.Models.Actions;
using MachineSteps.Models.Enums;
using MachineSteps.Models.Steps;
using MachineSteps.Plugins.IsoConverterBase;
using MachineSteps.Plugins.IsoIstructions;
using MachineSteps.Plugins.IsoParser.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace MachineSteps.Plugins.IsoParser.Converters.M
{
    [MIstructionConverter(250)]
    public class M250Converter : MIstructionConverter<State>
    {
        public override List<MachineStep> Convert(MIstruction istruction, State state)
        {
            return new List<MachineStep>()
            {
                new MachineStep()
                {
                    Id = GetStepId(),
                    Name = "Side push off",
                    Actions = new List<Models.Actions.BaseAction>
                    {
                        new TwoPositionLinkAction()
                        {
                            LinkId = 20,
                            RequestedState = TwoPositionLinkActionRequestedState.Off,
                        }
                    }
                }
            };
        }
    }
}
