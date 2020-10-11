using System.Collections;
using MachineSteps.Plugins.IsoConverterBase;
using MachineSteps.Plugins.IsoParser.Attributes;

namespace MachineSteps.Plugins.IsoParser.Converters.ETK
{
    [ETKConverter(726)]
    public class ETK726Converter : SetBitArrayIstructionConverter<State>
    {
        protected override BitArray GetState(State state) => state.Doweler2.Pressers;

        protected override void InitLinkIds()
        {
            _linkIds[0] = 11952;
            _linkIds[1] = 11953;
        }
    }
}
