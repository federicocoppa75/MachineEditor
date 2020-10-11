using System.Collections.Generic;
using MachineSteps.Models.Steps;
using MachineSteps.Plugins.IsoConverterBase;
using MachineSteps.Plugins.IsoInterpreter.Attributes;
using MachineSteps.Plugins.IsoIstructions;

namespace MachineSteps.Plugins.IsoInterpreter.Converters.VL
{
    [VLConverter(203)]
    public class VL203Converter : SetVariableIstructionConverter<State>
    {
        public override List<MachineStep> Convert(SetVariableIstruction istruction, State state)
        {
            state.Panel.Dz = istruction.Value;

            return null;
        }
    }
}
