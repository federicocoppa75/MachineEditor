using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineSteps.Plugins.IsoParser.Converters.StateData
{
    public class Clamps
    {
        public bool DxClamp { get; set; }

        public bool SxClamp { get; set; }

        public double DxDistance { get; set; }

        public double SxDistance { get; set; }
    }
}
