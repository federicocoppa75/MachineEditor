using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineSteps.Plugins.IsoParser.Converters.StateData
{
    public class RotoTranslMatrix
    {
        public double I { get; set; }
        public double J { get; set; }
        public double K { get; set; }
        public double P { get; set; }
        public double Q { get; set; }
        public double R { get; set; }
        public double U { get; set; }
        public double V { get; set; }
        public double W { get; set; }
        public double M { get; set; }
        public double N { get; set; }
        public double O { get; set; }

        public RotoTranslMatrix()
        {
            Reset();
        }

        public void Reset()
        {
            I = 1.0;
            J = 0.0;
            K = 0.0;
            P = 0.0;
            Q = 1.0;
            R = 0.0;
            U = 0.0;
            V = 0.0;
            W = 1.0;
            M = 0.0;
            N = 0.0;
            O = 1.0;
        }

        public bool IsIdentity()
        {
            bool result = false;

            if((I == 1.0)&&
                (J == 0.0)&&
                (K == 0.0)&&
                (P == 0.0)&&
                (Q == 1.0)&&
                (R == 0.0)&&
                (U == 0.0)&&
                (V == 0.0)&&
                (W == 1.0)&&
                (M == 0.0)&&
                (N == 0.0)&&
                (O == 1.0))
            {
                result = true;
            }

            return result;
        }

        public bool IsIdentityFlipped()
        {
            bool result = false;

            if (!IsIdentity())
            {
                if ((Math.Abs(I) == 1.0) &&
                    (J == 0.0) &&
                    (K == 0.0) &&
                    (P == 0.0) &&
                    (Math.Abs(Q) == 1.0) &&
                    (R == 0.0) &&
                    (U == 0.0) &&
                    (V == 0.0) &&
                    (Math.Abs(W) == 1.0) &&
                    (M == 0.0) &&
                    (N == 0.0) &&
                    (Math.Abs(O) == 1.0))
                {
                    result = true;
                }
            }

            return result;
        }

        public void Transform(Nullable<double> xIn, Nullable<double> yIn, Nullable<double> zIn, ref Nullable<double> xOut, ref Nullable<double> yOut, ref Nullable<double> zOut)
        {
            if(IsIdentity())
            {
                if (xIn.HasValue) xOut = xIn.Value;
                if (yIn.HasValue) yOut = yIn.Value;
                if (zIn.HasValue) zOut = zIn.Value;
            }
            else if(IsIdentityFlipped())
            {
                if (xIn.HasValue) xOut = xIn.Value * I;
                if (yIn.HasValue) yOut = yIn.Value * Q;
                if (zIn.HasValue) zOut = zIn.Value * W;
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public void Transform(double xIn, double yIn, ref double xOut, ref double yOut)
        {
            if (IsIdentity())
            {
                xOut = xIn;
                yOut = yIn;
            }
            else if (IsIdentityFlipped())
            {
                xOut = xIn * I;
                yOut = yIn * Q;
            }
            else
            {
                throw new NotImplementedException();
            }
        }
    }
}
