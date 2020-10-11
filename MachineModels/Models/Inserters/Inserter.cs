using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace MachineModels.Models.Inserters
{
    [Serializable]
    public class Inserter : BaseInserter
    {
        public double Diameter { get; set; }

        public double Length { get; set; }

        public int LoaderLinkId { get; set; }

        public int DischargerLinkId { get; set; }
    }
}
