using MachineSteps.Plugins.IsoInterpreter.Enums;

namespace MachineSteps.Plugins.IsoInterpreter.Models
{
    public class IsoLine
    {
        public int Number { get; set; }

        public string Data { get; set; }

        public virtual IsoLineType Type => IsoLineType.Unknown;
    }
}
