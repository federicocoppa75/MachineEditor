using System;

namespace MachineSteps.Plugins.IsoInterpreter.Messages
{
    public class GetActiveLevelOffsetMessage
    {
        public Action<double, double, double> AddOffset { get; set; }
    }
}
