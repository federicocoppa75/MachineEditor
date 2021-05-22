using System.Collections.Generic;
using System.Linq;
using MachineSteps.Models.Actions;
using MachineSteps.Models.Steps;
using MachineSteps.Plugins.IsoParser.Attributes;
using MachineSteps.Plugins.IsoIstructions;
using MachineSteps.Plugins.IsoConverterBase;

namespace MachineSteps.Plugins.IsoParser.Converters.G
{
    [GIstructionConverter(140)]
    public class G140Converter : GIstructionConverter<State>
    {
        const double _parkValue = 9999.0;

        public override List<MachineStep> Convert(GIstruction istruction, State state)
        {

            if(!IsSynchro(istruction))
            {
                var steps = new List<MachineStep>();

                ManagePreviousSynchro(istruction, state, steps);
                ManageNotOperation(istruction, state, steps);
                AddSynchroSteps(istruction, state, steps);

                return steps;
            }
            else
            {
                var steps = new List<MachineStep>();

                ManageWaiter(istruction, state, steps);

                return steps;
            }
        }

        private void ResetOffset(State state)
        {
            state.Axes.OX = 0.0;
            state.Axes.OY = 0.0;
            state.Axes.OZ = 0.0;
        }

        private void AddSynchroSteps(GIstruction istruction, State state, List<MachineStep> steps)
        {
            var moveStepXY = new MachineStep() { Id = GetStepId(), Name = "G140 - Move XY",  Actions = new List<BaseAction>(), Channel = 1 };
            var moveStepZ = new MachineStep() { Id = GetStepId(), Name = "G140 - Move Z",  Actions = new List<BaseAction>(), Channel = 1 };
            var synStep = new MachineStep() { Id= GetStepId(), Name = "G140 - Gantry", Actions = new List<BaseAction>(), Channel = 1 };
            bool invertOrder = false;

            if(istruction.Parameters.Keys.Contains('H'))
            {
                var h = istruction.Parameters['H'];

                if (h == 2.0) invertOrder = true;
            }

            if(istruction.Parameters.Keys.Contains('X') && istruction.Parameters.Keys.Contains('U') && istruction.Parameters.Keys.Contains('I'))
            {
                var x = istruction.Parameters['X'];
                var u = istruction.Parameters['U'];
                var i = istruction.Parameters['I'];
                var vs = false;

                if (x == _parkValue) { state.Axes.GetParkX(ref x); vs = true; }
                if (u == _parkValue) { state.Axes.GetParkU(ref u); vs = true; }
                state.Axes.SetXU(x, u, moveStepXY, false);
                state.Axes.SetGantryX(i, synStep, vs);
            }

            if (istruction.Parameters.Keys.Contains('Y') && istruction.Parameters.Keys.Contains('V') && istruction.Parameters.Keys.Contains('J'))
            {
                var y = istruction.Parameters['Y'];
                var v = istruction.Parameters['V'];
                var j = istruction.Parameters['J'];
                var vs = false;

                if (y == _parkValue) { state.Axes.GetParkY(ref y); vs = true; }
                if (v == _parkValue) { state.Axes.GetParkV(ref v); vs = true; }
                state.Axes.SetYV(y, v, moveStepXY, false);
                state.Axes.SetGantryY(j, synStep, vs);
            }

            if(istruction.Parameters.Keys.Contains('K'))
            {
                var parkZ = false;
                var parkW = false;
                var parkA = false;
                var parkB = false;

                if (istruction.Parameters.Keys.Contains('Z') && istruction.Parameters.Keys.Contains('W'))
                {
                    var z = istruction.Parameters['Z'];
                    var w = istruction.Parameters['W'];

                    if (z == _parkValue) { state.Axes.GetParkZ(ref z); parkZ = true; }
                    if (w == _parkValue) { state.Axes.GetParkW(ref w); parkW = true; }
                    state.Axes.SetZW(z, w, moveStepZ, false);
                }

                if(istruction.Parameters.Keys.Contains('A') && istruction.Parameters.Keys.Contains('B'))
                {
                    var a = istruction.Parameters['A'];
                    var b = istruction.Parameters['B'];

                    if (a == _parkValue) { state.Axes.GetParkA(ref a); parkA = true; }
                    if (b == _parkValue) { state.Axes.GetParkB(ref b); parkB = true; }
                    state.Axes.SetAB(a, b, moveStepZ, false);
                }

                var k = istruction.Parameters['K'];
                var vs = false;

                if (((k == 1) || (k == 2)) && (parkZ || parkW)) vs = true;
                else if (((k == 3) || (k == 4)) && (parkA || parkB)) vs = true;

                state.Axes.SetGantryZ(k, synStep, vs);
            }

            SetActionsIds(moveStepXY);
            SetActionsIds(moveStepZ);
            SetActionsIds(synStep);

            if(invertOrder)
            {
                if (moveStepZ.Actions.Count > 0) steps.Add(moveStepZ);
                if (moveStepXY.Actions.Count > 0) steps.Add(moveStepXY);                
            }
            else
            {
                if (moveStepXY.Actions.Count > 0) steps.Add(moveStepXY);
                if (moveStepZ.Actions.Count > 0) steps.Add(moveStepZ);
            }

            if (synStep.Actions.Count > 0) steps.Add(synStep);
        }

        private void ManagePreviousSynchro(GIstruction istruction, State state, List<MachineStep> steps)
        {
            var step = new MachineStep()
            {
                Id = GetStepId(),
                Name = "G140 - Reset previous",
                Actions = new List<BaseAction>()
            };
            var resetX = istruction.Parameters.Keys.Contains('X') && istruction.Parameters.Keys.Contains('U') && istruction.Parameters.Keys.Contains('I');
            var resetY = istruction.Parameters.Keys.Contains('Y') && istruction.Parameters.Keys.Contains('V') && istruction.Parameters.Keys.Contains('J');
            var resetZ = istruction.Parameters.Keys.Contains('Z') && istruction.Parameters.Keys.Contains('W') && istruction.Parameters.Keys.Contains('K');
            var resetZ2 = istruction.Parameters.Keys.Contains('A') && istruction.Parameters.Keys.Contains('B') && istruction.Parameters.Keys.Contains('K');

            if (resetX) state.Axes.ResetGantryX(step);
            if (resetY) state.Axes.ResetGantryY(step);

            if (resetZ || resetZ2)
            {
                state.Axes.ResetGantryZ(step);
                state.Axes.ResetGantryZ2(step);
            }

            if (step.Actions.Count > 0) steps.Add(step);
        }

        private void ManageNotOperation(GIstruction istruction, State state, List<MachineStep> steps)
        {
            if (steps.Count == 0)
            {
                var step = new MachineStep()
                {
                    Id = GetStepId(),
                    Name = "G140 - NOP",
                    Actions = new List<BaseAction>()
                };

                step.Actions.Add(new NotOperationAction() { Name = "NOP", Description = "NOP to start" });

                steps.Add(step);
            }     
        }

        private void ManageWaiter(GIstruction istruction, State state, List<MachineStep> steps)
        {
            var step = new MachineStep()
            {
                Id = GetStepId(),
                Name = "G140 - Synchro",
                Actions = new List<BaseAction>()
            };

            step.Actions.Add(new ChannelWaiterAction() { Name = "Waiter", Description = "Channel waiter", ChannelToWait = 1 });

            steps.Add(step);
        }

        private bool IsSynchro(GIstruction istruction)
        {
            return (istruction.Parameters != null) && istruction.Parameters.Keys.Contains('S');
        }
    }
}
