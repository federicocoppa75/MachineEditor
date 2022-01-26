using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace MachineSteps.Plugins.IsoParser.Converters.StateData
{
    public class Plane
    {
        public BitArray RoolerBars { get; private set; } = new BitArray(2);
    }
}
