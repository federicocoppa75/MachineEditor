using System.Collections;
using MachineSteps.Plugins.IsoConverterBase;
using MachineSteps.Plugins.IsoParser.Attributes;

namespace MachineSteps.Plugins.IsoParser.Converters.ETK
{
    [ETKConverter(3)]
    public class ETK3Converter : SetBitArrayIstructionConverter<State>
    {
        protected override void InitLinkIds()
        {
            _linkIds[0] = 9001;
            _linkIds[1] = 9002;
            _linkIds[2] = 9003;
            _linkIds[3] = 9004;
            _linkIds[4] = 9005;
            _linkIds[5] = 9006;
            _linkIds[6] = 9007;
            _linkIds[7] = 9008;
            _linkIds[8] = 9009;
            _linkIds[9] = 9010;
            _linkIds[10] = 9011;
            _linkIds[11] = 9012;
            _linkIds[12] = 9013;
        }

        protected override BitArray GetState(State state) => state.Driller2.Spindles[0];
    }
}
