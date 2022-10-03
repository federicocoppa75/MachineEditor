using HelixToolkit.Wpf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace MachineViewModels.Exporters
{
    internal class StlExporter
    {
        public static void Export(string fileName, MeshGeometryVisual3D visualMesh)
        {
            using (var stream = File.Create(fileName))
            {
                var writer = new BinaryWriter(stream);

                int triangleIndicesCount = 0;
                Traverse<GeometryModel3D>(visualMesh, Transform3D.Identity, (m, t) => triangleIndicesCount += GetTrianglesNum((MeshGeometry3D)m.Geometry));

                ExportHeader(writer, triangleIndicesCount / 3);
                Traverse<GeometryModel3D>(visualMesh, Transform3D.Identity, (m, t) => ExportModel(writer, m, t));
            }
        }

        private static int GetTrianglesNum(MeshGeometry3D mesh)
        {
            int result = 0;

            if (mesh != null)
            {
                result = mesh.TriangleIndices.Count;
            }

            return result;
        }

        private static void ExportHeader(BinaryWriter writer, int triangleCount)
        {
            writer.Write(new byte[80]);
            writer.Write(triangleCount);
        }

        private static readonly PropertyInfo Visual3DModelPropertyInfo = typeof(Visual3D).GetProperty("Visual3DModel", BindingFlags.Instance | BindingFlags.NonPublic);
        private static Model3D GetModel(Visual3D visual)
        {
            Model3D model;
            var mv = visual as ModelVisual3D;
            if (mv != null)
            {
                model = mv.Content;
            }
            else
            {
                model = Visual3DModelPropertyInfo.GetValue(visual, null) as Model3D;
            }

            return model;
        }

        private static IEnumerable<Visual3D> GetChildren(Visual3D parent)
        {
            int n = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < n; i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i) as Visual3D;
                if (child == null)
                {
                    continue;
                }

                yield return child;
            }
        }

        private static void Traverse<T>(Visual3D visual, Transform3D transform, Action<T, Transform3D> action)
            where T : Model3D
        {
            var childTransform = Transform3DHelper.CombineTransform(visual.Transform, transform);
            var model = GetModel(visual);
            if (model != null)
            {
                model.Traverse(childTransform, action);
            }

            foreach (var child in GetChildren(visual))
            {
                Traverse(child, childTransform, action);
            }
        }

        private static void ExportModel(BinaryWriter writer, GeometryModel3D model, Transform3D t)
        {
            if (model.Geometry == null) return;

            var mesh = (MeshGeometry3D)model.Geometry;

            if (mesh == null) return;

            var normals = mesh.Normals;
            if (normals == null || normals.Count != mesh.Positions.Count)
            {
                normals = MeshGeometryHelper.CalculateNormals(mesh);
            }

            // TODO: Also handle non-uniform scale
            var matrix = t.Clone().Value;
            matrix.OffsetX = 0;
            matrix.OffsetY = 0;
            matrix.OffsetZ = 0;
            var normalTransform = new MatrixTransform3D(matrix);

            var material = model.Material;
            var dm = material as DiffuseMaterial;

            var mg = material as MaterialGroup;
            if (mg != null)
            {
                foreach (var m in mg.Children)
                {
                    if (m is DiffuseMaterial)
                    {
                        dm = m as DiffuseMaterial;
                    }
                }
            }

            ushort attribute = 0;

            if (dm != null)
            {
                var scb = dm.Brush as SolidColorBrush;
                if (scb != null)
                {
                    byte r = scb.Color.R;
                    byte g = scb.Color.G;
                    byte b = scb.Color.B;
                    attribute = (ushort)((1 << 15) | ((r >> 3) << 10) | ((g >> 3) << 5) | (b >> 3));
                }
            }

            for (int i = 0; i < mesh.TriangleIndices.Count; i += 3)
            {
                int i0 = mesh.TriangleIndices[i + 0];
                int i1 = mesh.TriangleIndices[i + 1];
                int i2 = mesh.TriangleIndices[i + 2];

                // Normal
                var faceNormal = normalTransform.Transform(normals[i0] + normals[i1] + normals[i2]);
                faceNormal.Normalize();
                WriteVector(writer, faceNormal);

                // Vertices
                WriteVertex(writer, t.Transform(mesh.Positions[i0]));
                WriteVertex(writer, t.Transform(mesh.Positions[i1]));
                WriteVertex(writer, t.Transform(mesh.Positions[i2]));

                // Attributes
                writer.Write(attribute);
            }
        }

        private static void WriteVector(BinaryWriter writer, Vector3D normal)
        {
            writer.Write((float)normal.X);
            writer.Write((float)normal.Y);
            writer.Write((float)normal.Z);
        }

        private static void WriteVertex(BinaryWriter writer, Point3D p)
        {
            writer.Write((float)p.X);
            writer.Write((float)p.Y);
            writer.Write((float)p.Z);
        }

    }
}
