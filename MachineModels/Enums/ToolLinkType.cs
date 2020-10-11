using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineModels.Enums
{
    /// <summary>
    /// Tipo di connessione dell'utensile
    /// </summary>
    public enum ToolLinkType
    {
        None,
        Static,    // utensile montato manualemente
        Auto       // utensile che viene montato con cambio utensile automatico
    }
}
