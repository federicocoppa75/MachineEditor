using System;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace MachineViewModelUtils.Extensions
{
    static public class Media3DExtensions
    {
        public static void ChangeColor(this Model3D model, Color color)
        {
            if (model is Model3DGroup)
            {
                ChangeColor(model as Model3DGroup, color);
            }
            else if (model is GeometryModel3D)
            {
                ChangeColor(model as GeometryModel3D, color);
            }
        }

        private static void ChangeColor(GeometryModel3D model, Color color)
        {
            model.Material = model.BackMaterial = HelixToolkit.Wpf.MaterialHelper.CreateMaterial(color).Clone();
        }

        private static void ChangeColor(Model3DGroup model, Color color)
        {
            for (int i = 0; i < model.Children.Count; i++)
            {
                ChangeColor(model.Children[i], color);
            }
        }

        public static Color GetColor(this Model3D model)
        {
            if (model is Model3DGroup)
            {
                return GetColor(model as Model3DGroup);
            }
            else if (model is GeometryModel3D)
            {
                return GetColor(model as GeometryModel3D);
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        private static Color GetColor(GeometryModel3D model)
        {
            return GetColor(model.Material);
        }

        private static Color GetColor(Material material)
        {
            if (material is DiffuseMaterial)
            {
                return (material as DiffuseMaterial).Color;
            }
            else if (material is MaterialGroup)
            {
                return GetColor((material as MaterialGroup).Children[0]);
            }
            else if (material is EmissiveMaterial)
            {
                return (material as EmissiveMaterial).Color;
            }
            else if (material is SpecularMaterial)
            {
                return (material as SpecularMaterial).Color;
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        private static Color GetColor(Model3DGroup model)
        {
            return GetColor(model.Children[0]);
        }

        public static MatrixTransform3D GetRotationTransform(this MatrixTransform3D m)
        {
            var m3d = m.Value;

            m3d.OffsetX = 0.0;
            m3d.OffsetY = 0.0;
            m3d.OffsetZ = 0.0;

            return new MatrixTransform3D(m3d);
        }

        public static MatrixTransform3D GetTraslationTrasform(this MatrixTransform3D m)
        {
            var m3d = m.Value;

            m3d.M11 = 1.0;
            m3d.M12 = 0.0;
            m3d.M13 = 0.0;
            m3d.M14 = 0.0;
            m3d.M21 = 0.0;
            m3d.M22 = 1.0;
            m3d.M23 = 0.0;
            m3d.M24 = 0.0;
            m3d.M31 = 0.0;
            m3d.M32 = 0.0;
            m3d.M33 = 1.0;
            m3d.M34 = 0.0;
            m3d.M44 = 1.0;

            return new MatrixTransform3D(m3d);
        }

        public static Transform3D Invert(this Transform3D t)
        {
            var m = t.Value;

            m.Invert();
            //m.OffsetX *= -1;
            //m.OffsetY *= -1;
            //m.OffsetZ *= -1;

            return new MatrixTransform3D(m);
        }

        public static Transform3D StaticClone(this Transform3D t) => new MatrixTransform3D(t.Value);

        public static MeshGeometry3D GetMeshGeometry3D(this Model3DGroup modelGroup)
        {
            if (modelGroup != null && modelGroup.Children.Count == 1)
            {
                if (modelGroup.Children[0] is GeometryModel3D geometryModel)
                {
                    if (geometryModel.Geometry is MeshGeometry3D meshGeometry)
                    {
                        return meshGeometry;
                    }
                    else
                    {
                        throw new System.ArgumentException();
                    }
                }
                else
                {
                    throw new System.ArgumentException();
                }
            }
            else
            {
                throw new System.ArgumentException();
            }
        }

    }
}
