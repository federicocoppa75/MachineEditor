namespace MachineSteps.Plugins.IsoIstructions
{
    public class MIstruction : MachIstruction
    {
        public int Istruction { get; private set; }

        public MIstruction(int istruction)
        {
            Istruction = istruction;
        }
    }
}
