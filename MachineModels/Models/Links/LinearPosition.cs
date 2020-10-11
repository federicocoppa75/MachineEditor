using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineModels.Models.Links
{
    [Serializable]
    public class LinearPosition : DynamicLink
    {
        public double Min { get; set; }
        public double Max { get; set; }
        public double Pos { get; set; }
    }
}
