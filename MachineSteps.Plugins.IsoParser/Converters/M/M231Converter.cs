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
    [MIstructionConverter(231)]
    public class M231Converter : MIstructionConverter<State>
    {
        public override List<MachineStep> Convert(MIstruction istruction, State state)
        {
            var ar = state.Plane.RoolerBars;

            return new List<MachineStep>()
            {
                new MachineStep()
                {
                    Id = GetStepId(),
                    Name = "Roolbar UP",
                    Actions = new List<Models.Actions.BaseAction>
                    {
                        new TwoPositionLinkAction()
                        {
                            LinkId = 71,
                            RequestedState = ar[0] ? TwoPositionLinkActionRequestedState.On : TwoPositionLinkActionRequestedState.Off,
                        },
                        new TwoPositionLinkAction()
                        {
                            LinkId = 72,
                            RequestedState = ar[1] ? TwoPositionLinkActionRequestedState.On : TwoPositionLinkActionRequestedState.Off,
                        }
                    }
                }
            };
        }
    }
}
