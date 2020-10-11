using System.Collections;
using MachineSteps.Plugins.IsoConverterBase;
using MachineSteps.Plugins.IsoParser.Attributes;

namespace MachineSteps.Plugins.IsoParser.Converters.ETK
{
    [ETKConverter(2)]
    public class ETK2Converter : SetBitArrayIstructionConverter<State>
    {
        protected override void InitLinkIds() { }

        protected override BitArray GetState(State state) => state.Driller1.Spindles[2];
    }
}
