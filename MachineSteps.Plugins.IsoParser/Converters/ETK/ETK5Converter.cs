using System.Collections;
using MachineSteps.Plugins.IsoConverterBase;
using MachineSteps.Plugins.IsoParser.Attributes;

namespace MachineSteps.Plugins.IsoParser.Converters.ETK
{
    [ETKConverter(5)]
    public class ETK5Converter : SetBitArrayIstructionConverter<State>
    {
        protected override void InitLinkIds() { }

        protected override BitArray GetState(State state) => state.Driller2.Spindles[2];
    }
}
