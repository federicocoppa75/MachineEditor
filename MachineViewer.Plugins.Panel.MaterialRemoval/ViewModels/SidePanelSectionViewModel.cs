using g3;
using GalaSoft.MvvmLight.Threading;
using HelixToolkit.Wpf;
using MachineViewer.Plugins.Panel.MaterialRemoval.Enums;
using MachineViewer.Plugins.Panel.MaterialRemoval.Extensions;
using MachineViewer.Plugins.Panel.MaterialRemoval.Messages;
using MachineViewer.Plugins.Panel.MaterialRemoval.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace MachineViewer.Plugins.Panel.MaterialRemoval.ViewModels
{
    public class SidePanelSectionViewModel : PanelSectionViewModel
    {
        private int _processingSidePanel = 0;

        private SafeGeneratedMeshData _safeSideMeshData = new SafeGeneratedMeshData();

        protected GeometryModel3D _sideModel;

        protected ImplicitNaryDifference3d _processedSide;

        protected AxisAlignedBox3d _sideFilterBox;

        public override void Initialize()
        {
            base.Initialize();

            InitializeProcessedSide();
        }

        protected virtual void InitializeProcessedSide()
        {
            ImplicitFace face = CreateProcessedSide(Position);

            _sideFilterBox = face.Bounds();
            _sideFilterBox.Expand(0.0001);
            face.Width += 2.0;
            face.Height += 2.0;

            _processedSide = new ImplicitNaryDifference3d()
            {
                A = face,
                BSet = new List<BoundedImplicitFunction3d>()
            };
        }

        protected ImplicitFace CreateProcessedSide(PanelSectionPosition position)
        {
            return new ImplicitFace()
            {
                Origin = Center.ToVector3d() + GetSideCenterOffset(position),
                N = GetSideNormal(position),
                U = GetSideUDirection(position),
                V = Vector3d.AxisZ,
                Width = GetSideWidth(position),
                Height = SizeZ
            };
        }

        protected override void InitializeModel()
        {
            _sectionModel = CreateSectonModel();
            _sideModel = CreateSideModel(Position);

            var model = new Model3DGroup();

            model.Children.Add(_sectionModel);
            model.Children.Add(_sideModel);

            Model = model;
        }

        protected GeometryModel3D CreateSideModel(PanelSectionPosition position)
        {
            return new GeometryModel3D()
            {
                Geometry = CreateRect(position),
                Material = MaterialHelper.CreateMaterial(Colors.Orange)
            };
        }

        private MeshGeometry3D CreateRect(PanelSectionPosition position)
        {
            var builder = new MeshBuilder();
            var dx = SizeX / 2.0;
            var dy = SizeY / 2.0;
            var dz = SizeZ / 2.0;

            switch (position)
            {
                case PanelSectionPosition.SideBottom:
                    builder.AddQuad(new Point3D(Center.X - dx, Center.Y - dy, Center.Z +dz),
                                    new Point3D(Center.X - dx, Center.Y - dy, Center.Z - dz),
                                    new Point3D(Center.X + dx, Center.Y - dy, Center.Z - dz),
                                    new Point3D(Center.X + dx, Center.Y - dy, Center.Z + dz));
                    break;
                case PanelSectionPosition.SideTop:
                    builder.AddQuad(new Point3D(Center.X + dx, Center.Y + dy, Center.Z + dz),
                                    new Point3D(Center.X + dx, Center.Y + dy, Center.Z - dz),
                                    new Point3D(Center.X - dx, Center.Y + dy, Center.Z - dz),
                                    new Point3D(Center.X - dx, Center.Y + dy, Center.Z + dz));
                    break;
                case PanelSectionPosition.SideRight:
                    builder.AddQuad(new Point3D(Center.X + dx, Center.Y - dy, Center.Z + dz),
                                    new Point3D(Center.X + dx, Center.Y - dy, Center.Z - dz),
                                    new Point3D(Center.X + dx, Center.Y + dy, Center.Z - dz),
                                    new Point3D(Center.X + dx, Center.Y + dy, Center.Z + dz));
                    break;
                case PanelSectionPosition.SideLeft:
                    builder.AddQuad(new Point3D(Center.X - dx, Center.Y + dy, Center.Z + dz),
                                    new Point3D(Center.X - dx, Center.Y + dy, Center.Z - dz),
                                    new Point3D(Center.X - dx, Center.Y - dy, Center.Z - dz),
                                    new Point3D(Center.X - dx, Center.Y - dy, Center.Z + dz));
                    break;
                default:
                    throw new ArgumentException("The argument must be signle side position!");
            }

            return builder.ToMesh();
        }

        private void CutSectionSide(DMesh3 mesh, Vector3d origin, Vector3d direction, bool fill)
        {
            MeshPlaneCut cut = new MeshPlaneCut(mesh, origin, direction);
            bool bc = cut.Cut();

            if(fill)
            {
                PlanarHoleFiller filler = new PlanarHoleFiller(cut) { FillTargetEdgeLen = _cubeSize };
                bool bf = filler.Fill();
            }
        }

        private Vector3d GetSideNormal(PanelSectionPosition position)
        {
            Vector3d n;

            switch (position)
            {
                case PanelSectionPosition.SideBottom:
                    n = -Vector3d.AxisY;
                    break;
                case PanelSectionPosition.SideTop:
                    n = Vector3d.AxisY;
                    break;
                case PanelSectionPosition.SideRight:
                    n = Vector3d.AxisX;
                    break;
                case PanelSectionPosition.SideLeft:
                    n = -Vector3d.AxisX;
                    break;
                default:
                    throw new ArgumentException();
            }

            return n;
        }

        private Vector3d GetSideCenterOffset(PanelSectionPosition position)
        {
            Vector3d d;

            switch (position)
            {
                case PanelSectionPosition.SideBottom:
                    d = new Vector3d(0.0, -SizeY / 2.0, 0.0);
                    break;
                case PanelSectionPosition.SideTop:
                    d = new Vector3d(0.0, SizeY / 2.0, 0.0);
                    break;
                case PanelSectionPosition.SideRight:
                    d = new Vector3d(SizeX / 2.0, 0.0, 0.0);
                    break;
                case PanelSectionPosition.SideLeft:
                    d = new Vector3d(-SizeX / 2.0, 0.0, 0.0);
                    break;
                default:
                    throw new ArgumentException();
            }

            return d;
        }

        private Vector3d GetSideUDirection(PanelSectionPosition position)
        {
            Vector3d uDir;

            switch (position)
            {
                case PanelSectionPosition.SideBottom:
                    uDir = Vector3d.AxisX;
                    break;
                case PanelSectionPosition.SideTop:
                    uDir = -Vector3d.AxisX;
                    break;
                case PanelSectionPosition.SideRight:
                    uDir = Vector3d.AxisY;
                    break;
                case PanelSectionPosition.SideLeft:
                    uDir = -Vector3d.AxisY;
                    break;
                default:
                    throw new ArgumentException();
            }

            return uDir;
        }

        private double GetSideWidth(PanelSectionPosition position)
        {
            double w = 0.0;

            switch (position)
            {
                case PanelSectionPosition.SideBottom:
                case PanelSectionPosition.SideTop:
                    w = SizeX;
                    break;
                case PanelSectionPosition.SideRight:
                case PanelSectionPosition.SideLeft:
                    w = SizeY;
                    break;
                default:
                    throw new ArgumentException();
            }

            return w;
        }

        protected override void ProcessPendingExtension(List<BoundedImplicitFunction3d> pendingTools)
        {
            ProcessPendingExtensionImplementation(_processedSide, pendingTools, _sideFilterBox, (m) => GetMeshDataAsync(m, _safeSideMeshData));
        }

        protected void ProcessPendingExtensionImplementation(ImplicitNaryDifference3d processedSide, List<BoundedImplicitFunction3d> pendingTools, AxisAlignedBox3d filterBox, Action<DMesh3> setProcessedMesh)
        {
            var bounds = processedSide.Bounds();
            var indexList = new List<int>();

            for (int i = 0; i < pendingTools.Count; i++)
            {
                if (bounds.Intersects(pendingTools[i].Bounds())) indexList.Add(i);
            }

            if (indexList.Count > 0)
            {
                foreach (var idx in indexList) processedSide.BSet.Add(pendingTools[idx]);

                DMesh3 mesh = GenerateMeshBase(processedSide, filterBox);

                MeshPlaneCut cut = new MeshPlaneCut(mesh, Center.ToVector3d() + (Vector3d.AxisZ * SizeZ / 2.0), Vector3d.AxisZ);
                bool bc = cut.Cut();

                if (!mesh.IsCompact) mesh.CompactInPlace();

                setProcessedMesh(mesh);
            }
        }

        private DMesh3 GenerateMeshBase(BoundedImplicitFunction3d root, AxisAlignedBox3d filterBox)
        {
            MarchingCubes c = new MarchingCubes();
            c.Implicit = root;
            c.RootMode = MarchingCubes.RootfindingModes.LerpSteps;      // cube-edge convergence method
            c.RootModeSteps = 5;                                        // number of iterations
            c.Bounds = filterBox;//_sideFilterBox;
            c.CubeSize = _cubeSize / 4.0;
            c.Generate();
            MeshNormals.QuickCompute(c.Mesh);                           // generate normals

            return c.Mesh;
        }

        protected override void OnProcessPendingRemovalMessage(ProcessPendingRemovalMessage msg)
        {
            base.OnProcessPendingRemovalMessage(msg);

            _safeSideMeshData.ExeAndReset((positions, triangleIndices) =>
            {
                DispatcherHelper.CheckBeginInvokeOnUI(() =>
                {
                    _sideModel.Geometry = new MeshGeometry3D()
                    {
                        Positions = new Point3DCollection(positions),
                        TriangleIndices = new Int32Collection(triangleIndices)
                    };
                });
            });
        }

        protected override void AddRautingToProcessedPanel(SectionRoutToolMoveMessage msg, ImplicitRouting rout)
        {
            base.AddRautingToProcessedPanel(msg, rout);
            _processedSide.BSet.Add(rout);
        }

        protected override void ProcessPendingRout()
        {
            base.ProcessPendingRout();

            if (Interlocked.CompareExchange(ref _processingSidePanel, 1, 0) == 0)
            {
                Task.Run(() =>
                {
                    ProcessPendingRoutImplementation(_processedSide, _sideFilterBox, (m) => GetMeshDataAsync(m, _safeSideMeshData));
                    Interlocked.Exchange(ref _processingSidePanel, 0);
                });
            }            
        }

        protected void ProcessPendingRoutImplementation(ImplicitNaryDifference3d processedSide, AxisAlignedBox3d filterBox, Action<DMesh3> setProcessedMesh)
        {
            DMesh3 mesh = GenerateMeshBase(processedSide, filterBox);

            MeshPlaneCut cut = new MeshPlaneCut(mesh, Center.ToVector3d() + (Vector3d.AxisZ * SizeZ / 2.0), Vector3d.AxisZ);
            bool bc = cut.Cut();

            if (!mesh.IsCompact) mesh.CompactInPlace();

            setProcessedMesh(mesh);
        }

    }
}
