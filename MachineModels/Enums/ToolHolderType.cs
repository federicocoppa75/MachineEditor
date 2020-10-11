using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineModels.Enums
{
    [DefaultValue(None)]
    public enum ToolHolderType
    {
        None,
        Static,         // cambio utensile manuale
        AutoSource,     // porta utensile (posizione magazzino)
        AutoSink        // mandrino con cambio utensile automatico
    }
}
