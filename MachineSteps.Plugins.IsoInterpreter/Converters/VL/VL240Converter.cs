using MachineSteps.Models.Steps;
using MachineSteps.Plugins.IsoConverterBase;
using MachineSteps.Plugins.IsoInterpreter.Attributes;
using MachineSteps.Plugins.IsoIstructions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineSteps.Plugins.IsoInterpreter.Converters.VL
{
    [VLConverter(240)]
    public class VL240Converter : SetVariableIstructionConverter<State>
    {
        public override List<MachineStep> Convert(SetVariableIstruction istruction, State state)
        {
            state.Panel.Dy = istruction.Value;

            return null;
        }
    }
}
