using System.Collections;
using MachineSteps.Plugins.IsoConverterBase;
using MachineSteps.Plugins.IsoParser.Attributes;

namespace MachineSteps.Plugins.IsoParser.Converters.ETK
{
    [ETKConverter(0)]
    public class ETK0Converter : SetBitArrayIstructionConverter<State>
    {
        protected override void InitLinkIds()
        {
            _linkIds[0] = 8001;
            _linkIds[1] = 8002;
            _linkIds[2] = 8003;
            _linkIds[3] = 8004;
            _linkIds[4] = 8005;
            _linkIds[5] = 8006;
            _linkIds[6] = 8007;
            _linkIds[7] = 8008;
            _linkIds[8] = 8009;
            _linkIds[9] = 8010;
            _linkIds[10] = 8011;
            _linkIds[11] = 8012;
            _linkIds[12] = 8013;
            _linkIds[13] = 8014;
            _linkIds[14] = 8015;
            _linkIds[15] = 8016;
            _linkIds[16] = 8017;
            _linkIds[17] = 8018;
            _linkIds[18] = 8019;
            _linkIds[19] = 8020;
            _linkIds[20] = 8021;
        }

        protected override BitArray GetState(State state) => state.Driller1.Spindles[0];
    }
}
