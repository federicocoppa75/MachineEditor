using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineModels.Enums
{
    [DefaultValue(Static)]
    public enum LinkType
    {
        Static,

        LinearPosition,

        LinearPneumatic,

        RotaryPneumatic
    }
}
