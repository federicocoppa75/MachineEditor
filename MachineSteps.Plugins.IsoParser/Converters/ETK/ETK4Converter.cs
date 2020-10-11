using System.Collections;
using MachineSteps.Models.Steps;
using MachineSteps.Plugins.IsoConverterBase;
using MachineSteps.Plugins.IsoParser.Attributes;

namespace MachineSteps.Plugins.IsoParser.Converters.ETK
{
    [ETKConverter(4)]
    public class ETK4Converter : SetBitArrayIstructionConverter<State>
    {
        protected override void InitLinkIds()
        {
            _linkIds[0] = 9645;
            _linkIds[1] = 9545;
            _linkIds[2] = 9523;
            _linkIds[3] = 9623;
            _linkIds[4] = 9723;

            _linkIds[10] = 9645;
            _linkIds[11] = 9545;
            _linkIds[12] = 9523;
            _linkIds[13] = 9623;
            _linkIds[14] = 9723;

            _linkIds[31] = 9066;
        }

        protected override BitArray GetState(State state) => state.Driller2.Spindles[1];

        protected override void AddAction(MachineStep step, bool rqState, int linkId)
        {
            base.AddAction(step, rqState, linkId);

            if (linkId == 9066) base.AddAction(step, rqState, 9166);
        }
    }
}
