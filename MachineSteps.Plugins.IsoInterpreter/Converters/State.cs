using MachineSteps.Plugins.IsoInterpreter.Converters.StateData;
using MachineSteps.Plugins.IsoInterpreter.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineSteps.Plugins.IsoInterpreter.Converters
{
    public class State
    {
        public Panel Panel { get; private set; } = new Panel();

        public Axes Axes { get; private set; } = new Axes(0.0, 0.0, 167.0);

        public Clamp Clamp { get; private set; } = new Clamp();

        public Driller Driller { get; private set; } = new Driller();

        public LateralPresser LateralPresser { get; set; } = new LateralPresser();

        public int RotationSpeed { get; set; }

        public int FeedSpeed { get; set; }

        public ElectrospidleRotationState ElectrospidleRotationState { get; set; }
    }
}
