using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineSteps.Plugins.StepsViewer.Models
{
    public interface ILazyAction
    {
        bool IsUpdated { get; }

        void Update();
    }
}
