using MachineSteps.Models.Actions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineSteps.Models.Steps
{
    [Serializable]
    public class MachineStep : IMachineStep
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<BaseAction> Actions { get; set; }
        public int Channel { get; set; }
    }
}
