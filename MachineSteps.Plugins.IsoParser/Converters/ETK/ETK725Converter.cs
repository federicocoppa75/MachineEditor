using System.Collections;
using MachineSteps.Plugins.IsoConverterBase;
using MachineSteps.Plugins.IsoParser.Attributes;

namespace MachineSteps.Plugins.IsoParser.Converters.ETK
{
    [ETKConverter(725)]
    public class ETK725Converter : SetBitArrayIstructionConverter<State>
    {
        protected override BitArray GetState(State state) => state.Doweler1.Pressers;

        protected override void InitLinkIds()
        {
            _linkIds[0] = 10952;
            _linkIds[1] = 10953;
        }
    }
}
