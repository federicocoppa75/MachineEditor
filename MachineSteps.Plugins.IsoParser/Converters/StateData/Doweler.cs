using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineSteps.Plugins.IsoParser.Converters.StateData
{
    public class Doweler
    {
        public BitArray Pressers { get; private set; } = new BitArray(32);
    }
}
