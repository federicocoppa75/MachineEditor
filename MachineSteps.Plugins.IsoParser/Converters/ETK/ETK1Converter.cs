using System.Collections;
using MachineSteps.Models.Steps;
using MachineSteps.Plugins.IsoConverterBase;
using MachineSteps.Plugins.IsoParser.Attributes;

namespace MachineSteps.Plugins.IsoParser.Converters.ETK
{
    [ETKConverter(1)]
    public class ETK1Converter : SetBitArrayIstructionConverter<State>
    {
        protected override void InitLinkIds()
        {
            _linkIds[0] = 8645;
            _linkIds[1] = 8545;
            _linkIds[2] = 8523;
            _linkIds[3] = 8623;
            _linkIds[4] = 8723;

            _linkIds[10] = 8645;
            _linkIds[11] = 8545;
            _linkIds[12] = 8523;
            _linkIds[13] = 8623;
            _linkIds[14] = 8723;

            _linkIds[28] = 8093;
            _linkIds[29] = 8093;
            _linkIds[30] = 8093;
            _linkIds[31] = 8066;
        }

        protected override BitArray GetState(State state) => state.Driller1.Spindles[1];

        protected override void AddAction(MachineStep step, bool rqState, int linkId)
        {
            base.AddAction(step, rqState, linkId);

            if(linkId == 8066) base.AddAction(step, rqState, 8166);
        }
    }
}
