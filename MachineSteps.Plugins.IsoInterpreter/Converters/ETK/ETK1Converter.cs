using System.Collections;
using MachineSteps.Models.Steps;
using MachineSteps.Plugins.IsoConverterBase;
using MachineSteps.Plugins.IsoInterpreter.Attributes;

namespace MachineSteps.Plugins.IsoInterpreter.Converters.ETK
{
    [ETKConverter(1)]
    public class ETK1Converter : SetBitArrayIstructionConverter<State>
    {
        protected override BitArray GetState(State state) => state.Driller.Spindles[1];


        protected override void InitLinkIds()
        {
            _linkIds[0] = 1623;
            _linkIds[1] = 1601;
            _linkIds[2] = 1623;
            _linkIds[3] = 1589;
            _linkIds[4] = 1082;
        }

        protected override void AddAction(MachineStep step, bool rqState, int linkId)
        {
            base.AddAction(step, rqState, linkId);

            if (linkId == 1082) base.AddAction(step, rqState, 1182);
        }
    }
}
