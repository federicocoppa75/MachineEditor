using HelixToolkit.Wpf;
using MachineModels.Models.Tools;
using MachineViewModelUtils.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace MachineViewModelUtils.Helpers
{
    public static class ToolsMeshHelper
    {
        public static Material ToolMaterial => Materials.Blue;

        public static Brush ToolBrush => Brushes.Blue;

        public static ModelVisual3D GetSimpleModel(Tool tool, Point3D position, Vector3D direction, Func<string, MeshGeometryVisual3D> visual3DFactory = null)
        {
            return GetToolModel(tool, position, direction, GetSimpleMesh, visual3DFactory);
        }

        public static ModelVisual3D GetTwoSectionModel(Tool tool, Point3D position, Vector3D direction, Func<string, MeshGeometryVisual3D> visual3DFactory = null)
        {
            return GetToolModel(tool, position, direction, GetTwoSectioMesh, visual3DFactory);
        }

        public static ModelVisual3D GetPointedModel(Tool tool, Point3D position, Vector3D direction, Func<string, MeshGeometryVisual3D> visual3DFactory = null)
        {
             return GetToolModel(tool, position, direction, GetPointedMesh, visual3DFactory);

        }

        public static ModelVisual3D GetDiskModel(Tool tool, Point3D position, Vector3D direction, Func<string, MeshGeometryVisual3D> visual3DFactory = null)
        {
             return GetToolModel(tool, position, direction, GetDiskMesh, visual3DFactory);
        }

        public static ModelVisual3D GetCountersinkModel(Tool tool, Point3D position, Vector3D direction, Func<string, MeshGeometryVisual3D> visual3DFactory = null)
        {
            return GetToolModel(tool, position, direction, GetCountersinkMesh, visual3DFactory);
        }

        public static ModelVisual3D GetDiskOnConeModel(Tool tool, Point3D position, Vector3D direction, Func<string, MeshGeometryVisual3D> visual3DFactory = null)
        {
             return GetToolModel(tool, position, direction, GetDiskOnConeMesh, visual3DFactory);
        }

        public static ModelVisual3D GetToolModel(Tool tool, Point3D position, Vector3D direction, Func<Tool, Point3D, Vector3D, MeshGeometry3D> getGeometry, Func<string, MeshGeometryVisual3D> visual3DFactory = null)
        {
            var t = (visual3DFactory != null) ? visual3DFactory(tool.Name) : new MeshGeometryVisual3D();

            if (tool.ToolLinkType == MachineModels.Enums.ToolLinkType.Auto && System.IO.File.Exists(tool.ConeModelFile))
            {
                t.MeshGeometry = getGeometry(tool, new Point3D(), direction);
                t.Material = Materials.Blue;
                t.Fill = Brushes.Blue;

                var cg = LoadModelMeshGeometry(tool.ConeModelFile);
                var cm = (visual3DFactory != null) ? visual3DFactory("cono") : new MeshGeometryVisual3D();

                cm.MeshGeometry = cg;
                cm.Material = Materials.DarkGray;
                cm.Fill = Brushes.DimGray;

                var tr = new TranslateTransform3D() { OffsetX = position.X, OffsetY = position.Y, OffsetZ = position.Z };

                t.Children.Add(cm);
                t.Transform = tr;

                return t;
            }
            else
            {
                t.MeshGeometry = getGeometry(tool, position, direction);
                t.Material = Materials.Blue;
                t.Fill = Brushes.Blue;

                return t;
            }
        }

        public static MeshGeometry3D GetSimpleMesh(Tool tool, Point3D position, Vector3D direction)
        {
            var t = tool as SimpleTool;
            var builder = new HelixToolkit.Wpf.MeshBuilder();
            var p = position + direction * t.Length;

            builder.AddCylinder(position, p, t.Diameter / 2.0);

            return builder.ToMesh();
        }

        public static MeshGeometry3D GetPointedMesh(Tool tool, Point3D position, Vector3D direction)
        {
            var t = tool as PointedTool;
            var builder = new HelixToolkit.Wpf.MeshBuilder();
            var p1 = position + direction * t.StraightLength;
            var p2 = position + direction * (t.StraightLength + t.ConeHeight);

            builder.AddCylinder(position, p1, t.Diameter / 2.0);
            builder.AddCone(p1, p2, t.Diameter / 2.0, false, 20);

            return builder.ToMesh();
        }

        public static MeshGeometry3D GetTwoSectioMesh(Tool tool, Point3D position, Vector3D direction)
        {
            var t = tool as TwoSectionTool;
            var builder = new HelixToolkit.Wpf.MeshBuilder();
            var p1 = position + direction * t.Length1;
            var p2 = position + direction * (t.Length1 + t.Length2);

            builder.AddCylinder(position, p1, t.Diameter1 / 2.0);
            builder.AddCylinder(p1, p2, t.Diameter2 / 2.0);

            return builder.ToMesh();
        }

        public static MeshGeometry3D GetDiskMesh(Tool tool, Point3D position, Vector3D direction)
        {
            var t = tool as DiskTool;
            var builder = new HelixToolkit.Wpf.MeshBuilder();
            var d = Math.Abs(t.BodyThickness - t.CuttingThickness) / 2.0;
            var r1 = t.Diameter / 2.0 - t.CuttingRadialThickness;
            var profile = new Point[]
            {
                new Point(t.BodyThickness, 10.0),
                new Point(t.BodyThickness, r1),
                new Point(t.BodyThickness + d, r1),
                new Point(t.BodyThickness + d, t.Diameter / 2.0),
                new Point(- d, t.Diameter / 2.0),
                new Point(- d, r1),
                new Point(0.0, r1),
                new Point(0.0, 10.0)
            };

            builder.AddRevolvedGeometry(profile, null, position, direction, 100);

            return builder.ToMesh();
        }

        public static MeshGeometry3D GetCountersinkMesh(Tool tool, Point3D position, Vector3D direction)
        {
            const double hSvasatore = 10.0;
            var t = tool as CountersinkTool;
            var builder = new HelixToolkit.Wpf.MeshBuilder();
            var p1 = position + direction * (t.Length1 - hSvasatore);
            var p12 = position + direction * t.Length1;
            var p2 = position + direction * (t.Length1 + t.Length2);
            var p3 = position + direction * (t.Length1 + t.Length2 + t.Length3);

            builder.AddCylinder(position, p1, t.Diameter1 / 2.0);
            builder.AddCylinder(p1, p12, t.Diameter2 / 2.0);
            builder.AddCone(p12, direction, t.Diameter2 / 2.0, t.Diameter1 / 2.0, t.Length2, false, false, 20);
            builder.AddCylinder(p2, p3, t.Diameter1 / 2.0);

            return builder.ToMesh();
        }

        public static MeshGeometry3D GetDiskOnConeMesh(Tool tool, Point3D position, Vector3D direction)
        {
            var t = tool as DiskOnConeTool;
            var builder = new HelixToolkit.Wpf.MeshBuilder();
            var d = Math.Abs(t.BodyThickness - t.CuttingThickness) / 2.0;
            var r1 = t.Diameter / 2.0 - t.CuttingRadialThickness;
            var p1 = position + direction * t.PostponemntLength;
            var profile = new Point[]
            {
                new Point(t.BodyThickness, t.PostponemntDiameter / 2.0),
                new Point(t.BodyThickness, r1),
                new Point(t.BodyThickness + d, r1),
                new Point(t.BodyThickness + d, t.Diameter / 2.0),
                new Point(- d, t.Diameter / 2.0),
                new Point(- d, r1),
                new Point(0.0, r1),
                new Point(0.0, t.PostponemntDiameter / 2.0)
            };

            builder.AddRevolvedGeometry(profile, null, p1, direction, 100);
            builder.AddCylinder(position, p1, t.PostponemntDiameter, 20);

            return builder.ToMesh();
        }

        public static MeshGeometry3D LoadModelMeshGeometry(string modelFile)
        {
            var mi = new ModelImporter();
            var m = mi.Load(modelFile, null);

            return m.GetMeshGeometry3D();
        }

        public static ModelVisual3D GetModelFromFile(string modelFile, Point3D position, Func<MeshGeometryVisual3D> visual3DFactory = null)
        {
            var atg = LoadModelMeshGeometry(modelFile);
            var atm = (visual3DFactory != null) ? visual3DFactory() : new MeshGeometryVisual3D();
            var tr = new TranslateTransform3D() { OffsetX = position.X, OffsetY = position.Y, OffsetZ = position.Z };

            atm.MeshGeometry = atg;
            atm.Material = Materials.DarkGray;
            atm.Fill = Brushes.DimGray;

            atm.Transform = tr;

            return atm;
        }
    }
}
