using System.Collections;
using MachineSteps.Plugins.IsoConverterBase;
using MachineSteps.Plugins.IsoInterpreter.Attributes;

namespace MachineSteps.Plugins.IsoInterpreter.Converters.ETK
{
    [ETKConverter(0)]
    public class ETK0Converter : SetBitArrayIstructionConverter<State>
    {
        protected override BitArray GetState(State state) => state.Driller.Spindles[0];


        protected override void InitLinkIds()
        {
            _linkIds[0] = 1001;
            _linkIds[1] = 1002;
            _linkIds[2] = 1003;
            _linkIds[3] = 1004;
            _linkIds[4] = 1005;
            _linkIds[5] = 1006;
            _linkIds[6] = 1007;
            _linkIds[7] = 1008;
            _linkIds[8] = 1009;
            _linkIds[9] = 1010;
            _linkIds[10] = 1011;
            _linkIds[11] = 1012;

            _linkIds[30] = 1589;
            _linkIds[31] = 1601;
        }
    }
}
