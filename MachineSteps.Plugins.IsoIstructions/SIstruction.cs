namespace MachineSteps.Plugins.IsoIstructions
{
    public class SIstruction : MachIstruction
    {
        public int Value { get; private set; }

        public SIstruction(int value)
        {
            Value = value;
        }
    }
}
