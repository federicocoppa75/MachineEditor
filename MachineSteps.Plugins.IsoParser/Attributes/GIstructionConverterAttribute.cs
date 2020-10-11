using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineSteps.Plugins.IsoParser.Attributes
{
    [AttributeUsage(AttributeTargets.Class, Inherited = true)]
    public class GIstructionConverterAttribute : IndexedBaseConverterAttribute
    {
        public GIstructionConverterAttribute(int index) : base(index)
        {
        }
    }
}
