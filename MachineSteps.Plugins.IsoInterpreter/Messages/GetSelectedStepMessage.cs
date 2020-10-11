using MachineSteps.Plugins.IsoInterpreter.Models;
using System;

namespace MachineSteps.Plugins.IsoInterpreter.Messages
{
    public class GetSelectedStepMessage
    {
        public Action<IsoLine> SetStep { get; set; }
    }
}
