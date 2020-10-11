using System.Collections.Generic;
using System.Linq;
using MachineSteps.Models.Actions;
using MachineSteps.Models.Enums;
using MachineSteps.Models.Steps;
using MachineSteps.Plugins.IsoParser.Attributes;
using MachineSteps.Plugins.IsoIstructions;
using MachineSteps.Plugins.IsoConverterBase;

namespace MachineSteps.Plugins.IsoParser.Converters.ETK
{
    [ETKConverter(714)]
    public class ETK714Converter : SetVariableIstructionConverter<State>
    {
        public override List<MachineStep> Convert(SetVariableIstruction istruction, State state)
        {
            bool clamp1 = false;
            bool clamp2 = false;
            var v = istruction.Value;
            var step = new MachineStep()
            {
                Id = GetStepId(),
                Name = "ETK[714]",
                Actions = new List<BaseAction>()
            };

            if(v == 1.0)
            {
                clamp1 = true;
            }
            else if(v == 2.0)
            {
                clamp2 = true;
            }
            else if(v == 3.0)
            {
                clamp1 = true;
                clamp2 = true;
            }

            if(clamp1 ^ state.Clamps.SxClamp)
            {
                step.Actions.Add(new TwoPositionLinkAction()
                {
                    LinkId = 11,
                    RequestedState = clamp1 ? TwoPositionLinkActionRequestedState.On : TwoPositionLinkActionRequestedState.Off
                });

                state.Clamps.SxClamp = clamp1;
            }

            if(clamp2 ^ state.Clamps.DxClamp)
            {
                step.Actions.Add(new TwoPositionLinkAction()
                {
                    LinkId = 12,
                    RequestedState = clamp2 ? TwoPositionLinkActionRequestedState.On : TwoPositionLinkActionRequestedState.Off
                });

                state.Clamps.DxClamp = clamp2;
            }

            SetActionsIds(step);

            return (step.Actions.Count() > 0) ? new List<MachineStep>() { step } : null;
        }
    }
}
