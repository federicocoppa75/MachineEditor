using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineEditor.Messages
{
    class ChangeModelLocation
    {
        /// <summary>
        /// Variazione della posizione lungo l'asse X.
        /// </summary>
        public double DX { get; set; }

        /// <summary>
        /// Variazione della posizione lungo l'asse Y.
        /// </summary>
        public double DY { get; set; }

        /// <summary>
        /// Variazione della posizione lungo l'asse Z.
        /// </summary>
        public double DZ { get; set; }
    }
}
