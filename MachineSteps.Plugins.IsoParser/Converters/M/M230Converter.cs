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
    [MIstructionConverter(230)]
    public class M230Converter : MIstructionConverter<State>
    {
        public override List<MachineStep> Convert(MIstruction istruction, State state)
        {
            for (int i = 0; i < state.Plane.RoolerBars.Length; i++) state.Plane.RoolerBars[i] = true;

            return new List<MachineStep>() 
            {
                new MachineStep() 
                { 
                    Id = GetStepId(), 
                    Name = "Roolbar reset",
                    Actions = new List<Models.Actions.BaseAction> 
                    {
                        new TwoPositionLinkAction()
                        {
                            LinkId = 71,
                            RequestedState = TwoPositionLinkActionRequestedState.Off
                        },
                        new TwoPositionLinkAction()
                        {
                            LinkId = 72,
                            RequestedState = TwoPositionLinkActionRequestedState.Off
                        }
                    }
                }
            };
        }
    }
}
