using MachineModels.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineModels.Models.PanelHolders
{
    [Serializable]
    public class PanelHolder
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public Vector Position { get; set; }

        public PanelLoadType Corner { get; set; }
    }
}
