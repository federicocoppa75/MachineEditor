using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineSteps.Models.Actions
{
    public interface IBaseAction
    {
        int Id { get; set; }
        string Name { get; set; }
        string Description { get; set; }
    }
}
