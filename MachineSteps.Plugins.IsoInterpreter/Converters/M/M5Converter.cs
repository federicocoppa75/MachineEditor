using System.Collections.Generic;
using MachineSteps.Models.Steps;
using MachineSteps.Plugins.IsoConverterBase;
using MachineSteps.Plugins.IsoInterpreter.Attributes;
using MachineSteps.Plugins.IsoInterpreter.Enums;
using MachineSteps.Plugins.IsoIstructions;

namespace MachineSteps.Plugins.IsoInterpreter.Converters.M
{
    [MIstructionConverter(5)]
    public class M5Converter : MIstructionConverter<State>
    {
        public override List<MachineStep> Convert(MIstruction istruction, State state)
        {
            if(state.ElectrospidleRotationState == ElectrospidleRotationState.On)
            {
                state.ElectrospidleRotationState = ElectrospidleRotationState.Off;
            }

            return base.Convert(istruction, state);
        }
    }
}
