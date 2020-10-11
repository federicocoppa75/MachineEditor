using System;

namespace MachineSteps.Plugins.IsoInterpreter.Attributes
{
    [AttributeUsage(AttributeTargets.Class, Inherited = true)]
    public class EDKConverterAttribute : SetVariableIstructionConverterAttribute
    {
        public EDKConverterAttribute(int index) : base("EDK", index)
        {
        }
    }
}
