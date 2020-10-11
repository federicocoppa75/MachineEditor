using MachineModels.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace MachineModels.Models.Colliders
{
    [XmlInclude(typeof(PointsCollider))]
    [Serializable]
    public class Collider
    {
        public ColliderType Type { get; set; }
    }
}
