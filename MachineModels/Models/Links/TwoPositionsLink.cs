using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineModels.Models.Links
{
    [Serializable]
    public class TwoPositionsLink : DynamicLink
    {
        public double OffPos { get; set; }
        public double OnPos { get; set; }

        /// <summary>
        /// Intervallo di tempo per passare dalla posizione ON alla posizione OFF (millisecondi).
        /// </summary>
        public double TOff { get; set; } = 0.2;

        /// <summary>
        /// Intervallo di tempo per passare dalla posizione OFF alla posizione ON (millisecondi).
        /// </summary>
        public double TOn { get; set; } = 0.2;

        /// <summary>
        /// Flag che indica che l'attivazione del link implica l'attivazione dell'utensile sottostante
        /// </summary>
        public bool ToolActivator { get; set; }
    }
}
