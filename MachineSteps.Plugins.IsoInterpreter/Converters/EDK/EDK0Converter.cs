using MachineSteps.Plugins.IsoConverterBase;
using MachineSteps.Plugins.IsoInterpreter.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineSteps.Plugins.IsoInterpreter.Converters.EDK
{
    [EDKConverter(0)]
    public class EDK0Converter : SetVariableIstructionConverter<State>
    {
    }
}
