using System.Collections.Generic;
using MachineSteps.Models.Actions;
using MachineSteps.Models.Enums;
using MachineSteps.Models.Steps;
using MachineSteps.Plugins.IsoConverterBase;
using MachineSteps.Plugins.IsoInterpreter.Attributes;
using MachineSteps.Plugins.IsoIstructions;

namespace MachineSteps.Plugins.IsoInterpreter.Converters.M
{
    [MIstructionConverter(431)]
    public class M431Converter : MIstructionConverter<State>
    {
        public override List<MachineStep> Convert(MIstruction istruction, State state)
        {
            var steps = new List<MachineStep>();

            AttivaBattuta(steps);
            CaricaPannello(state, steps);

            return steps;
        }

        private static void CaricaPannello(State state, List<MachineStep> steps)
        {
            if (state.Panel.Dx > 0.0 && state.Panel.Dy > 0.0 && state.Panel.Dz > 0.0)
            {
                steps.Add(new MachineStep()
                {
                    Id = GetStepId(),
                    Name = "M431 - LoadPanel",
                    Actions = new List<BaseAction>()
                    {
                        new AddPanelAction()
                        {
                            Name = "Load panel",
                            CornerReference = PanelCornerReference.Corner2,
                            PanelHolder = 0,
                            XDimension = state.Panel.Dx,
                            YDimension = state.Panel.Dy,
                            ZDimension = state.Panel.Dz
                        }
                    }
                });
            }
        }

        private static void AttivaBattuta(List<MachineStep> steps)
        {
            steps.Add(new MachineStep()
            {
                Id = GetStepId(),
                Name = "M431 - Activate",
                Actions = new List<BaseAction>()
                {
                    new TwoPositionLinkAction()
                    {
                        LinkId = 10,
                        RequestedState = TwoPositionLinkActionRequestedState.On
                    }
                }
            });
        }
    }
}
