namespace MachineSteps.Plugins.IsoIstructions
{
    public class SetVariableIstruction : BaseIstruction 
    {
        public string Name { get; set; }
        public int Index { get; private set; }
        public double Value { get; set; }

        public SetVariableIstruction(string name, int index, double value)
        {
            Name = name;
            Index = index;
            Value = value;
        }
    }
}
