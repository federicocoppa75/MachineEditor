using System;

namespace MachineSteps.Plugins.IsoInterpreter.Attributes
{
    [AttributeUsage(AttributeTargets.Class, Inherited = true)]
    public class VLConverterAttribute : SetVariableIstructionConverterAttribute
    {
        public VLConverterAttribute(int index) : base("VL", index)
        {
        }
    }
}
