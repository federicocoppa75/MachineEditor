using MachineSteps.Plugins.IsoInterpreter.Models;
using System.Collections.Generic;

namespace MachineSteps.Plugins.IsoInterpreter.Messages
{
    public class SetAxesParametersMessage
    {
        public List<Parameter> Parameters { get; set; }
    }
}
