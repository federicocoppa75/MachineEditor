using g3;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MachineViewer.Plugins.Panel.MaterialRemoval.Models
{
    public class ImplicitFace : BoundedImplicitFunction3d
    {
        public Vector3d Origin { get; set; }

        public Vector3d N { get; set; }

        public Vector3d U { get; set; }

        public Vector3d V { get; set; }

        public double Width { get; set; }

        public double Height { get; set; }

        public AxisAlignedBox3d Bounds()
        {
            var corners = GetCorners();
            Vector3d min = corners[0];
            Vector3d max = corners[0];

            for (int i = 1; i < 4; i++)
            {
                if (min.x > corners[i].x) min.x = corners[i].x;
                if (min.y > corners[i].y) min.y = corners[i].y;
                if (min.z > corners[i].z) min.z = corners[i].z;
                if (max.x < corners[i].x) max.x = corners[i].x;
                if (max.y < corners[i].y) max.y = corners[i].y;
                if (max.z < corners[i].z) max.z = corners[i].z;
            }

            return new AxisAlignedBox3d(min, max /*+ N * MathUtil.Epsilon*/);
        }

        public double Value(ref Vector3d pt)
        {
            var halfW = Width / 2.0;
            var halfH = Height / 2.0;
            var d = pt - Origin;
            var du = U.Dot(ref d);
            var dv = V.Dot(ref d);
            var uuul = du <= halfW;
            var uull = du >= -halfW;
            var uvul = dv <= halfH;
            var uvll = dv >= -halfH;
            var nc = N.Dot(ref d);
            double result = 0.0;

            if (uuul && uull && uvll && uvul)
            {
                result = nc;
            }
            else if(uuul && uull)
            {
                var pc = Math.Abs(dv) - halfH;
                result = Math.Sqrt(pc * pc + nc * nc);
            }
            else if(uvll && uvul)
            {
                var pc = Math.Abs(du) - halfW;
                result = Math.Sqrt(pc * pc + nc * nc);
            }
            else
            {
                var cu = Math.Abs(du) - halfW;
                var cv = Math.Abs(dv) - halfH;

                result = Math.Sqrt(nc * nc + cu * cu + cv * cv);
            }

            return result;
        }

        private Vector3d[] GetCorners()
        {
            return new Vector3d[] 
            {
                Origin - U * Width / 2.0 - V * Height / 2.0,
                Origin + U * Width / 2.0 - V * Height / 2.0,
                Origin + U * Width / 2.0 + V * Height / 2.0,
                Origin - U * Width / 2.0 + V * Height / 2.0
            };
        }
    }
}
