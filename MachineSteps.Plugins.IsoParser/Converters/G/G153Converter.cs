using System;
using System.Collections.Generic;
using System.Linq;
using MachineSteps.Models.Actions;
using MachineSteps.Models.Steps;
using MachineSteps.Plugins.IsoParser.Attributes;
using MachineSteps.Plugins.IsoIstructions;
using MachineSteps.Plugins.IsoConverterBase;

namespace MachineSteps.Plugins.IsoParser.Converters.G
{
    [GIstructionConverter(153)]
    public class G153Converter : GIstructionConverter<State>
    {
        public override List<MachineStep> Convert(GIstruction istruction, State state)
        {
            double g = -1;

            istruction.Parameters.TryGetValue('G', out g);

            var step = new MachineStep()
            {
                Id = GetStepId(),
                Name = $"G153 G{g}",
                Actions = new List<BaseAction>()
            };

            switch (g)
            {
                case 0:
                    ConvertG0(istruction, state, step);
                    break;

                case 1:
                    ConvertG1(istruction, state, step);
                    break;

                default:
                    throw new NotImplementedException($"G153 G{g} not implemented!");
            }

            SetActionsIds(step);
            SetMaxDuration(step);

            return (step.Actions.Count() > 0) ? new List<MachineStep>() { step } : null;
        }

        private static void ConvertG0(GIstruction istruction, State state, MachineStep step)
        {
            if(state.Axes.GantryX == StateData.Axes.Gantry.None)
            {
                var moveX = istruction.Parameters.TryGetValue('X', out double x);
                var moveU = istruction.Parameters.TryGetValue('U', out double u);

                if (moveX && moveU)
                {
                    state.Axes.SetXU(x, u, step, false);
                }
                else if (moveX)
                {
                    state.Axes.SetX(x, step, false);
                }
                else if(moveU)
                {
                    state.Axes.SetU(u, step, false);
                }
            }
            else
            {
                if (istruction.Parameters.TryGetValue('X', out double x)) state.Axes.SetRapidX(x, step, false);
            }

            if (state.Axes.GantryY == StateData.Axes.Gantry.None)
            {
                var moveY = istruction.Parameters.TryGetValue('Y', out double y);
                var moveV = istruction.Parameters.TryGetValue('V', out double v);

                if(moveY && moveV)
                {
                    state.Axes.SetYV(y, v, step, false);
                }
                else if(moveY)
                {
                    state.Axes.SetY(y, step, false);
                }
                else if(moveV)
                {
                    state.Axes.SetV(v, step, false);
                }
            }
            else
            {
                if (istruction.Parameters.TryGetValue('Y', out double y)) state.Axes.SetRapidY(y, step, false);
            }

            if ((state.Axes.GantryZ == StateData.Axes.Gantry.None) && (state.Axes.GantryZ2 == StateData.Axes.Gantry.None))
            {
                var moveZ = istruction.Parameters.TryGetValue('Z', out double z);
                var moveW = istruction.Parameters.TryGetValue('W', out double w);
                var moveA = istruction.Parameters.TryGetValue('A', out double a);
                var moveB = istruction.Parameters.TryGetValue('B', out double b);

                if (moveZ && moveW && moveA && moveB)
                {
                    state.Axes.SetZWAB(z, w, a, b, step, false);
                }
                else if (moveZ && moveW)
                {
                    state.Axes.SetZW(z, w, step, false);
                }
                else if (moveA && moveB)
                {
                    state.Axes.SetAB(a, b, step, false);
                }
                else if(moveZ)
                {
                    state.Axes.SetZ(z, step, false);
                }
                else if(moveW)
                {
                    state.Axes.SetW(w, step, false);
                }
                else if (moveA)
                {
                    state.Axes.SetA(a, step, false);
                }
                else if (moveB)
                {
                    state.Axes.SetB(b, step, false);
                }
            }
            else
            {
                if (istruction.Parameters.TryGetValue('Z', out double z)) state.Axes.SetRapidZ(z, step, false);
            }


        }

        private static void ConvertG1(GIstruction istruction, State state, MachineStep step)
        {
            var x = new Nullable<double>();
            var y = new Nullable<double>();
            var z = new Nullable<double>();
            double v = 0.0;

            if (istruction.Parameters.TryGetValue('X', out v)) x = v;
            if (istruction.Parameters.TryGetValue('Y', out v)) y = v;
            if (istruction.Parameters.TryGetValue('Z', out v)) z = v;
            if (istruction.Parameters.TryGetValue('F', out v)) state.FeedSpeed = (int)v;

            state.Axes.SetPosition(step, state.FeedSpeed, x, y, z, false);
        }
    }
}
