using System.Collections.Generic;
using MachineSteps.Models.Steps;
using MachineSteps.Plugins.IsoConverterBase;
using MachineSteps.Plugins.IsoInterpreter.Attributes;
using MachineSteps.Plugins.IsoInterpreter.Enums;
using MachineSteps.Plugins.IsoIstructions;

namespace MachineSteps.Plugins.IsoInterpreter.Converters.M
{
    [MIstructionConverter(3)]
    public class M3Converter : MIstructionConverter<State>
    {
        public override List<MachineStep> Convert(MIstruction istruction, State state)
        {
            if(state.ElectrospidleRotationState == ElectrospidleRotationState.Off)
            {
                state.ElectrospidleRotationState = ElectrospidleRotationState.On;
            }

            return base.Convert(istruction, state);            
        }
    }
}
