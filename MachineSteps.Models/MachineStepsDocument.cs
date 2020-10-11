using MachineSteps.Models.Steps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineSteps.Models
{
    [Serializable]
    public class MachineStepsDocument
    {
        public List<MachineStep> Steps { get; set; }
    }
}
