namespace MachineSteps.Plugins.IsoInterpreter.Messages
{
    public class IfThenLineMessage
    {
        public int LineNumber{ get; set; }

        public string Condition { get; set; }

        public int ElseLineNumber { get; set; }

        public int EndLineNumber { get; set; }
    }
}
