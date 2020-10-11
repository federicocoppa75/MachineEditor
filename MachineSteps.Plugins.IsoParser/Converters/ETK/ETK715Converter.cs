using System.Collections;
using MachineSteps.Plugins.IsoConverterBase;
using MachineSteps.Plugins.IsoParser.Attributes;

namespace MachineSteps.Plugins.IsoParser.Converters.ETK
{
    [ETKConverter(715)]
    public class ETK715Converter : SetBitArrayIstructionConverter<State>
    {
        protected override void InitLinkIds()
        {
            _linkIds[0] = 8952;
            _linkIds[1] = 8953;
            _linkIds[2] = 8962;
            _linkIds[3] = 8963;
            _linkIds[4] = 8972;
            _linkIds[5] = 8973;
            _linkIds[6] = 8954;
            _linkIds[7] = 8955;
            _linkIds[8] = 8964;
            _linkIds[9] = 8965;
            _linkIds[10] = 8911;
        }

        protected override BitArray GetState(State state) => state.Driller1.Pressers;
    }
}
