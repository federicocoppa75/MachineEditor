using System.Collections.Generic;
using MachineSteps.Models.Steps;
using MachineSteps.Plugins.IsoParser.Attributes;
using MachineSteps.Plugins.IsoIstructions;
using MachineSteps.Plugins.IsoConverterBase;
using System;

namespace MachineSteps.Plugins.IsoParser.Converters.G
{
    [GIstructionConverter(305)]
    public class G305Converter : GIstructionConverter<State>
    {
        public override List<MachineStep> Convert(GIstruction istruction, State state)
        {
            if(istruction.Parameters.TryGetValue('S', out double s))
            {
                if(s == 0.0)
                {
                    state.Axes.M.Reset();
                }
                else if(s == 1.0)
                {
                    if (istruction.Parameters.TryGetValue('I', out double i) &&
                        istruction.Parameters.TryGetValue('J', out double j) &&
                        istruction.Parameters.TryGetValue('K', out double k) &&
                        istruction.Parameters.TryGetValue('P', out double p) &&
                        istruction.Parameters.TryGetValue('Q', out double q) &&
                        istruction.Parameters.TryGetValue('R', out double r) &&
                        istruction.Parameters.TryGetValue('U', out double u) &&
                        istruction.Parameters.TryGetValue('V', out double v) &&
                        istruction.Parameters.TryGetValue('W', out double w) &&
                        istruction.Parameters.TryGetValue('M', out double m) &&
                        istruction.Parameters.TryGetValue('N', out double n) &&
                        istruction.Parameters.TryGetValue('O', out double o))
                    {
                        state.Axes.M.I = i;
                        state.Axes.M.J = j;
                        state.Axes.M.K = k;
                        state.Axes.M.P = p;
                        state.Axes.M.Q = q;
                        state.Axes.M.R = r;
                        state.Axes.M.U = u;
                        state.Axes.M.V = v;
                        state.Axes.M.W = w;
                        state.Axes.M.M = m;
                        state.Axes.M.N = n;
                        state.Axes.M.O = o;
                    }
                    else
                    {
                        throw new ArgumentException("Invalid parameters set in G305!");
                    }
                }
                else
                {
                    throw new ArgumentException("Parameter S in G305 invalid value!");
                }
            }

            return null;
        }
    }
}
