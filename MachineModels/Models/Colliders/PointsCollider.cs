using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineModels.Models.Colliders
{
    [Serializable]
    public class PointsCollider : Collider
    {
        public double Radius { get; set; } = 10.0;

        public List<Vector> Points { get; set; } = new List<Vector>();
    }
}
