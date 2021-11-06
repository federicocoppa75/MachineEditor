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
                if ((xIn.HasValue && (I != 0.0)) || (yIn.HasValue && (P != 0.0)) || (zIn.HasValue && (U != 0.0)))
                {
                    // ATTENZIONE: le componenti Y e Z locali proiettate sulla X del sistema macchina sono moltipicati per un fattore -1.0
                    //             perché la X è inverita per via della movimentazione a pinza, la componente X è già invertita nell'ISO
                    xOut = (xIn.HasValue ? xIn.Value * I : 0.0) + (yIn.HasValue ? yIn.Value * P * (-1.0) : 0.0) + (zIn.HasValue ? zIn.Value * U * (-1.0): 0.0);
                }
                else
                {
                    xOut = null;
                }

                if ((xIn.HasValue && (J != 0.0)) || (yIn.HasValue && (Q != 0.0)) || (zIn.HasValue && (V != 0.0)))
                {
                    // ATTENZIONE: la componente X locale proiettata dulla Y globale è moltiplicata per un fattore -1.0 perché nell'ISO è invertita (per il movimento tramite pinza)
                    yOut = (xIn.HasValue ? xIn.Value * J * (-1.0) : 0.0) + (yIn.HasValue ? yIn.Value * Q : 0.0) + (zIn.HasValue ? zIn.Value * V : 0.0);
                }
                else
                {
                    yOut = null;
                }

                if ((xIn.HasValue && (K != 0.0)) || (yIn.HasValue && (R != 0.0)) || (zIn.HasValue && (W != 0.0)))
                {
                    // ATTENZIONE: la componente X locale proiettata dulla Z globale è moltiplicata per un fattore -1.0 perché nell'ISO è invertita (per il movimento tramite pinza)
                    zOut = (xIn.HasValue ? xIn.Value * K * (-1.0) : 0.0) + (yIn.HasValue ? yIn.Value * R : 0.0) + (zIn.HasValue ? zIn.Value * W : 0.0);
                }
                else
                {
                    zOut = null;
                }
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
