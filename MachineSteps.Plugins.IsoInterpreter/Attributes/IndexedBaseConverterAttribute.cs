using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineSteps.Plugins.IsoInterpreter.Attributes
{
    [AttributeUsage(AttributeTargets.Class, Inherited = true)]
    public class IndexedBaseConverterAttribute : BaseConvertAttribute
    {
        public int Index { get; private set; }

        public IndexedBaseConverterAttribute(int index)
        {
            Index = index;
        }
    }
}
