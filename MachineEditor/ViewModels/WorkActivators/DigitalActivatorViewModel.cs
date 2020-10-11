using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineEditor.ViewModels.WorkActivators
{
    public class DigitalActivatorViewModel : BaseActivatorViewModel
    {
        public double SwitchOnInterval { get; set; }

        public double SwitchOffInterval { get; set; }

        public double ScwitchOnValue { get; set; }

        public double SwitchOffValue { get; set; }
    }
}
