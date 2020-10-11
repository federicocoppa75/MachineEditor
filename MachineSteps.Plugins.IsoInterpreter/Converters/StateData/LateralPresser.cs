using MachineSteps.Plugins.IsoInterpreter.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineSteps.Plugins.IsoInterpreter.Converters.StateData
{
    public class LateralPresser
    {
        public LateralPresserState LateralPresserState { get; set; }

        public LateralPresserPressureState LateralPresserPressureState { get; set; }
    }
}
