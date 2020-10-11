using MachineSteps.Plugins.IsoInterpreter.Enums;

namespace MachineSteps.Plugins.IsoInterpreter.Messages
{
    public class ShiftValueChangedMessage
    {
        public ShiftDirection Direction { get; set; }

        public double Value { get; set; }
    }
}
