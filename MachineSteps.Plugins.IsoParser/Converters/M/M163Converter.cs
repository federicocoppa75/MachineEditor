using System.Collections.Generic;
using MachineSteps.Models.Actions;
using MachineSteps.Models.Enums;
using MachineSteps.Models.Steps;
using MachineSteps.Plugins.IsoParser.Attributes;
using MachineSteps.Plugins.IsoIstructions;
using MachineSteps.Plugins.IsoConverterBase;

namespace MachineSteps.Plugins.IsoParser.Converters.M
{
    [MIstructionConverter(163)]
    public class M163Converter : MIstructionConverter<State>
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
                    Name = "M163 - LoadPanel",
                    Actions = new List<BaseAction>()
                    {
                        new AddPanelAction()
                        {
                            Name = "Load panel",
                            CornerReference = PanelCornerReference.Corner1,
                            PanelHolder = 1,
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
                Name = "M163 - Activate",
                Actions = new List<BaseAction>()
                {
                    new TwoPositionLinkAction()
                    {
                        LinkId = 52,
                        RequestedState = TwoPositionLinkActionRequestedState.On
                    }
                }
            });
        }
    }
}
