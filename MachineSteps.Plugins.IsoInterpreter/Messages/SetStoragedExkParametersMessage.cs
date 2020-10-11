using MachineSteps.Plugins.IsoInterpreter.Models;
using System.Collections.Generic;

namespace MachineSteps.Plugins.IsoInterpreter.Messages
{
    public class SetStoragedExkParametersMessage
    {
        public List<Parameter> Parameters { get; set; }
    }
}
