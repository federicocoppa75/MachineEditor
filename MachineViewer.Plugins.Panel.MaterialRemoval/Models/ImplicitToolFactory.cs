using g3;
using MachineViewer.Plugins.Panel.MaterialRemoval.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace MachineViewer.Plugins.Panel.MaterialRemoval.Models
{
    public static class ImplicitToolFactory
    {
        public static ImplicitToolBase Create(Point3D position, Vector3D direction, double length, double radius)
        {
            return Create(position.ToVector3d(), direction.ToVector3d(), length, radius);
        }

        public static ImplicitToolBase Create(Vector3d position, Vector3d direction, double length, double radius)
        {
            ImplicitToolBase result = null;

            if((direction.x == 0) && (direction.y == 0))
            {
                result = new ImplicitToolZ(position, length, radius, direction.z > 0);
            }
            else if ((direction.z == 0) && (direction.y == 0))
            {
                result = new ImplicitToolX(position, length, radius, direction.x > 0);
            }
            else if ((direction.x == 0) && (direction.z == 0))
            {
                result = new ImplicitToolY(position, length, radius, direction.y > 0);
            }
            else
            {
                throw new NotImplementedException();
            }

            return result;
        }
    }
}
