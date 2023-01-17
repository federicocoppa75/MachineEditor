using MachineSteps.Models.Steps;
using MachineSteps.Plugins.IsoConverterBase;
using MachineSteps.Plugins.IsoIstructions;
using MachineSteps.Plugins.IsoParser.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace MachineSteps.Plugins.IsoParser.Converters.ETK
{
    [ETKConverter(70)]
    public class ETK70Converter : SetVariableIstructionConverter<State>
    {
        public override List<MachineStep> Convert(SetVariableIstruction istruction, State state)
        {
            var v = (int)istruction.Value;
            
            state.HeadSetup = (v == 0) ? 1 : 0;
            state.HeadSetupSelected = true;

            return null;
        }
    }
}
