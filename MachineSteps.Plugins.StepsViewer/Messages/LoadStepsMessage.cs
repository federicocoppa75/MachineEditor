using MachineSteps.Models.Steps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineSteps.Plugins.StepsViewer.Messages
{
    public class LoadStepsMessage
    {
        public List<MachineStep> Steps { get; set; }
    }
}
