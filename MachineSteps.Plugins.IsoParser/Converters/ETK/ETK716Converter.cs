using System.Collections;
using MachineSteps.Plugins.IsoConverterBase;
using MachineSteps.Plugins.IsoParser.Attributes;

namespace MachineSteps.Plugins.IsoParser.Converters.ETK
{
    [ETKConverter(716)]
    public class ETK716Converter : SetBitArrayIstructionConverter<State>
    {
        protected override void InitLinkIds()
        {
            _linkIds[0] = 9952;
            _linkIds[1] = 9953;
            _linkIds[2] = 9962;
            _linkIds[3] = 9963;
            _linkIds[4] = 9972;
            _linkIds[5] = 9955;
            _linkIds[6] = 9965;
        }

        protected override BitArray GetState(State state) => state.Driller2.Pressers;
    }
}
