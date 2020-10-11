using System;

namespace MachineSteps.Plugins.IsoInterpreter.Messages
{
    public class GetAxisValueMessage
    {
        public string AxisName { get; set; }
        public Action<double> SetValue { get; set; }
        public Action<bool> SetReady { get; set; }
        public Action SetFinded { get; set; }
    }
}
