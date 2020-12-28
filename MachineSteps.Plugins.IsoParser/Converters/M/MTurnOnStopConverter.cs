using MachineSteps.Models.Actions;
using MachineSteps.Models.Enums;
using MachineSteps.Models.Steps;
using MachineSteps.Plugins.IsoConverterBase;
using MachineSteps.Plugins.IsoIstructions;
using System.Collections.Generic;

namespace MachineSteps.Plugins.IsoParser.Converters.M
{
    public class MTurnOnStopConverter : MIstructionConverter<State>
    {
        public string Name { get; set; }
        public int LinkId { get; set; }
        public int PanelHolder { get; set; }
        public PanelCornerReference CornerReference { get; set; }

        public override List<MachineStep> Convert(MIstruction istruction, State state)
        {
            var steps = new List<MachineStep>();

            AttivaBattuta(state, steps);
            CaricaPannello(state, steps);

            return steps;
        }

        private void CaricaPannello(State state, List<MachineStep> steps)
        {
            if (state.Panel.Dx > 0.0 && state.Panel.Dy > 0.0 && state.Panel.Dz > 0.0)
            {
                steps.Add(new MachineStep()
                {
                    Id = GetStepId(),
                    //Name = "M164 - LoadPanel",
                    Actions = new List<BaseAction>()
                    {
                        new AddPanelAction()
                        {
                            Name = "Load panel",
                            CornerReference = CornerReference,
                            PanelHolder = PanelHolder,
                            XDimension = state.Panel.Dx,
                            YDimension = state.Panel.Dy,
                            ZDimension = state.Panel.Dz
                        }
                    }
                });
            }
        }

        private void AttivaBattuta(State state, List<MachineStep> steps)
        {
            steps.Add(new MachineStep()
            {
                Id = GetStepId(),
                Name = $"{Name} - Activate",
                Actions = new List<BaseAction>()
                {
                    new TwoPositionLinkAction()
                    {
                        LinkId = LinkId,
                        RequestedState = TwoPositionLinkActionRequestedState.On
                    }
                }
            });

            state.StopLinkTurnedOn = LinkId;
        }
    }
}
