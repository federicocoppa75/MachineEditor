using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineSteps.Plugins.IsoParser
{
    public static class StateInfoServices
    {
        public static Func<int, Tuple<double, double>> GetLinkLimits { get; set; }
    }
}
