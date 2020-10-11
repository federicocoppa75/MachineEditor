using g3;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineViewer.Plugins.Panel.MaterialRemoval.Models
{
    public static class ImplicitRoutingFactory
    {
        public static ImplicitRouting Create(int toolId, Vector3d direction, double length, double radius)
        {
            ImplicitRouting result = null;

            if ((direction.x == 0) && (direction.y == 0))
            {
                result = new ImplicitRoutingZ(length, radius, toolId, direction.z > 0);
            }
            else if ((direction.z == 0) && (direction.y == 0))
            {
                result = new ImplicitRoutingX(length, radius, toolId, direction.x > 0);
            }
            else if ((direction.x == 0) && (direction.z == 0))
            {
                result = new ImplicitRoutingY(length, radius, toolId, direction.y > 0);
            }
            else
            {
                throw new NotImplementedException();
            }

            return result;
        }
    }
}
