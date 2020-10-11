using System;
using System.Collections.Generic;

namespace MachineSteps.Plugins.IsoInterpreter.Models
{
    [Serializable]
    public class ParametersSet
    {
        public List<Parameter> AxesParameters { get; set; }
        public List<Parameter> StoragedExkParameters { get; set; }
    }
}
