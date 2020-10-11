using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineSteps.Plugins.IsoInterpreter.Messages.Conversion
{
    public class SetVariableMessage : BaseIstructionMessage
    {
        public string Name { get; set; }
        public string Value { get; set; }
    }
}
