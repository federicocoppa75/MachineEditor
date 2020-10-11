using System.Collections;
using MachineSteps.Plugins.IsoConverterBase;
using MachineSteps.Plugins.IsoInterpreter.Attributes;

namespace MachineSteps.Plugins.IsoInterpreter.Converters.ETK
{
    [ETKConverter(715)]
    public class ETK715Converter : SetBitArrayIstructionConverter<State>
    {
        protected override BitArray GetState(State state) => state.Driller.Pressers;

        protected override void InitLinkIds()
        {
            _linkIds[0] = 1101;
            _linkIds[1] = 1102;
            _linkIds[2] = 1103;
            _linkIds[3] = 1104;
        }
    }
}
