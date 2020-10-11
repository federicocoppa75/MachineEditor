using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineViewer.Plugins.Injectors.SimpleManipolator.Messages
{
    public class GetAvailablaInjectorsMessage
    {
        public Action<int> SetInjectorData { get; set; }
    }
}
