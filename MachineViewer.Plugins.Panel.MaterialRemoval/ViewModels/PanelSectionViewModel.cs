using g3;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Threading;
using HelixToolkit.Wpf;
using MachineViewer.Plugins.Panel.MaterialRemoval.Enums;
using MachineViewer.Plugins.Panel.MaterialRemoval.Extensions;
using MachineViewer.Plugins.Panel.MaterialRemoval.Messages;
using MachineViewer.Plugins.Panel.MaterialRemoval.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace MachineViewer.Plugins.Panel.MaterialRemoval.ViewModels
{
    public class PanelSectionViewModel : ViewModelBase
    {
        protected class SafeGeneratedMeshData
        {
            ConcurrentQueue<Tuple<List<Point3D>, List<int>>> _queue = new ConcurrentQueue<Tuple<List<Point3D>, List<int>>>();

            public void Set(List<Point3D> positions, List<int> triangleIndeces)
            {
                _queue.Enqueue(new Tuple<List<Point3D>, List<int>>(positions, triangleIndeces));
            }

            public void ExeAndReset(Action<List<Point3D>, List<int>> action)
            {
                if (!_queue.IsEmpty && _queue.TryDequeue(out Tuple<List<Point3D>, List<int>> t))
                {
                    action(t.Item1, t.Item2);
                }
            }
        }

        private static int _seedId = 0;

        protected double _processingOffset = 2.0;

        private object _queueLock = new object();

        private int _processingPanel = 0;

        protected List<BoundedImplicitFunction3d> _pendingTools;

        private ImplicitNaryDifference3d _processedPanel;

        protected AxisAlignedBox3d _filterBox;

        protected double _cubeSize;

        protected Vector3d _min;

        protected Vector3d _max;

        protected GeometryModel3D _sectionModel;

        private SafeGeneratedMeshData _generatedMeshData = new SafeGeneratedMeshData();

        private ImplicitRouting _lastRouting;

        private static object _routLock = new object();

        public Model3D Model { get; set; }

        public int Id { get; set; }

        public int XSectionIndex { get; set; }

        public int YSectionIndex { get; set; }

        public PanelSectionPosition Position { get; set; }

        public Point3D Center { get; set; }

        public double SizeX { get; set; }

        public double SizeY { get; set; }

        public double SizeZ { get; set; }

        public Model3D FilterBox
        {
            get
            {
                var filterBox = new Rect3D(_filterBox.Min.x, _filterBox.Min.y, _filterBox.Min.z, _filterBox.Width, _filterBox.Height, _filterBox.Depth);
                var builder = new MeshBuilder();

                builder.AddBoundingBox(filterBox, 0.2);

                return new GeometryModel3D()
                {
                    Geometry = builder.ToMesh(),
                    Material = MaterialHelper.CreateMaterial(Colors.Cyan)
                };
            }
        }

        public Model3D BoundingBox
        {
            get
            {
                var box = new Rect3D(Center.X - SizeX / 2.0, Center.Y - SizeY / 2.0, Center.Z - SizeZ / 2.0, SizeX, SizeY, SizeZ);
                var builder = new MeshBuilder();

                builder.AddBoundingBox(box, 0.1);

                return new GeometryModel3D()
                {
                    Geometry = builder.ToMesh(),
                    Material = MaterialHelper.CreateMaterial(Colors.Brown)
                };
            }
        }

        private int _numcells;
        public int NumCells
        {
            get => _numcells;
            set => _numcells = value;
        }

        public PanelSectionViewModel()
        {
            Id = _seedId++;
            MessengerInstance.Register<SectionToolMoveMessage>(this, OnSectionToolMoveMessage);
            MessengerInstance.Register<ProcessPendingRemovalMessage>(this, OnProcessPendingRemovalMessage);
            MessengerInstance.Register<SectionRoutToolMoveMessage>(this, OnSectionRoutToolMoveMessage);
        }

        public virtual void Initialize()
        {
            var p = Center.ToVector3d() - new Vector3d(SizeX / 2.0, SizeY / 2.0, SizeZ / 2.0);
            var d = new Vector3d(SizeX, SizeY, SizeZ);
            var offset = _processingOffset;
            var offsetV = new Vector3d(offset, offset, 0.0);

            _min = p;
            _max = p + d;

            InitializeMeshGenerationData();

            _processedPanel = new ImplicitNaryDifference3d()
            {
                A = new ImplicitBox3d()
                {
                    Box = new Box3d(new AxisAlignedBox3d(_min - offsetV, _max + offsetV))
                },
                BSet = new List<BoundedImplicitFunction3d>()
            };

            InitializeModel();
        }

        protected virtual void InitializeMeshGenerationData()
        {
            InitializeCubeSize();

            _filterBox = new AxisAlignedBox3d(_min, _max);
            _filterBox.Max.x -= _cubeSize / 2.0;
            _filterBox.Max.y -= _cubeSize / 2.0;
        }

        protected void InitializeCubeSize()
        {
            var filterBox = new AxisAlignedBox3d(_min, _max);
            _cubeSize = filterBox.MaxDim / _numcells;
        }

        protected virtual void InitializeModel()
        {
            _sectionModel = CreateSectonModel();

            Model = _sectionModel;
        }

        protected GeometryModel3D CreateSectonModel()
        {
            var builder = new MeshBuilder();

            builder.AddBox(Center, SizeX, SizeY, SizeZ);

            return new GeometryModel3D()
            {
                Geometry = builder.ToMesh(),
                Material = MaterialHelper.CreateMaterial(Colors.Orange)
            };
        }

        private void OnSectionToolMoveMessage(SectionToolMoveMessage msg)
        {
            if((msg.XSectionIndex == XSectionIndex) && (msg.YSectionIndex == YSectionIndex))
            {
                var tool = msg.Tool;

                Task.Run(() =>
                {
                    lock(_queueLock)
                    {
                        if (_pendingTools == null) _pendingTools = new List<BoundedImplicitFunction3d>();

                        _pendingTools.SmartAdd(tool);
                    }

                    Task.Run(() => ProcessPendings());
                });
            }
        }

        private void OnSectionRoutToolMoveMessage(SectionRoutToolMoveMessage msg)
        {
            if((msg.XSectionIndex == XSectionIndex) && (msg.YSectionIndex == YSectionIndex))
            {
                var rout = msg.Rout;

                Task.Run(() =>
                {
                    bool add = true;

                    lock (_routLock)
                    {
                        if ((_lastRouting != null) && (_lastRouting.Id == rout.Id) && (_lastRouting.ToolId == rout.ToolId))
                        {
                            var lastIndex = _processedPanel.BSet.Count - 1;
                            var last = (lastIndex >= 0) ? _processedPanel.BSet[lastIndex] : null;

                            if ((last != null) && (last is ImplicitRouting ir) && (ir.Id == rout.Id) && (ir.ToolId == rout.ToolId))
                            {
                                //_processedPanel.BSet[lastIndex] = msg.Rout;
                                // teoricamente l'oggetto dovrebbe essere già aggiornato con il nuovo tool
                                add = false;
                            }
                        }

                        if (add)
                        {
                            AddRautingToProcessedPanel(msg, rout);
                        }
                    }

                    Task.Run(() => ProcessPendingRout());
                });
            }
        }

        protected virtual void AddRautingToProcessedPanel(SectionRoutToolMoveMessage msg, ImplicitRouting rout)
        {
            _processedPanel.BSet.Add(msg.Rout);
            _lastRouting = rout;
        }

        protected virtual void OnProcessPendingRemovalMessage(ProcessPendingRemovalMessage msg)
        {
             _generatedMeshData.ExeAndReset((positions, triangleIndices) =>
            {
                DispatcherHelper.CheckBeginInvokeOnUI(() =>
                {
                    _sectionModel.Geometry = new MeshGeometry3D()
                    {
                        Positions = new Point3DCollection(positions),
                        TriangleIndices = new Int32Collection(triangleIndices)
                    };
                });
            });
        }

        private void ProcessPendings()
        {
            if (Interlocked.CompareExchange(ref _processingPanel, 1, 0) == 0)
            {
                Task.Run(() =>
                {
                    List<BoundedImplicitFunction3d> pendingTools = null;

                    lock (_queueLock)
                    {
                        pendingTools = _pendingTools;
                        _pendingTools = null;
                    }

                    if (pendingTools != null)
                    {
                        var added = _processedPanel.BSet.SmartAddRange(pendingTools);

                        if (added)
                        {
                            GenerateMesh(_processedPanel);
                            ProcessPendingExtension(pendingTools);
                        }
                    }

                    Interlocked.Exchange(ref _processingPanel, 0);
                });
            }
        }

        protected virtual void ProcessPendingRout()
        {
            if (Interlocked.CompareExchange(ref _processingPanel, 1, 0) == 0)
            {
                Task.Run(() =>
                {
                    GenerateMesh(_processedPanel);
                    Interlocked.Exchange(ref _processingPanel, 0);
                });
            }
        }

        private void GenerateMesh(BoundedImplicitFunction3d root)
        {
            DMesh3 mesh = GenerateMeshBase(root);

            if (!mesh.IsCompact) mesh.CompactInPlace();

            GetMeshDataAsync(mesh, _generatedMeshData);
        }

        private DMesh3 GenerateMeshBase(BoundedImplicitFunction3d root)
        {
            MarchingCubes c = new MarchingCubes();
            c.Implicit = root;
            c.RootMode = MarchingCubes.RootfindingModes.LerpSteps;      // cube-edge convergence method
            c.RootModeSteps = 5;                                        // number of iterations
            c.Bounds = _filterBox;
            c.CubeSize = _cubeSize;
            c.Generate();
            MeshNormals.QuickCompute(c.Mesh);                           // generate normals

            return c.Mesh;
        }

        protected virtual void ProcessPendingExtension(List<BoundedImplicitFunction3d> pendingTools){ }

        protected Task<List<Point3D>> GetPositionsAsync(DMesh3 mesh)
        {
            return Task.Run(() =>
            {
                var positions = new List<Point3D>(mesh.VerticesRefCounts.count);
                var vertices = mesh.VerticesBuffer;

                foreach (int vId in mesh.VerticesRefCounts)
                {
                    int i = vId * 3;
                    positions.Add(new Point3D(vertices[i], vertices[i + 1], vertices[i + 2]));
                }

                return positions;
            });
        }

        protected Task<List<int>> GetTriangleIndicesAsync(DMesh3 mesh)
        {
            return Task.Run(() =>
            {
                var tringleindices = new List<int>(mesh.TrianglesRefCounts.count);
                var triangles = mesh.TrianglesBuffer;

                foreach (int tId in mesh.TrianglesRefCounts)
                {
                    int i = tId * 3;
                    tringleindices.Add(triangles[i]);
                    tringleindices.Add(triangles[i + 1]);
                    tringleindices.Add(triangles[i + 2]);
                }

                return tringleindices;
            });
        }

        protected Task GetMeshDataAsync(DMesh3 mesh, SafeGeneratedMeshData meshData)
        {
            var md = meshData;
            List<Point3D> positions = null;
            List<int> triangleIndices = null;
            var tasks = new Task[]
            {
                GetPositionsAsync(mesh).ContinueWith((t) => positions = t.Result),
                GetTriangleIndicesAsync(mesh).ContinueWith((t) => triangleIndices = t.Result)
            };

            return Task.WhenAll(tasks).ContinueWith((t) => md.Set(positions, triangleIndices));
        }

        protected void UpdateModel(GeometryModel3D geometryModel, List<Point3D> positions, List<int> trianglesIndices)
        {
            var meshGeometry = geometryModel.Geometry as MeshGeometry3D;

            meshGeometry.Positions.Clear();
            meshGeometry.TriangleIndices.Clear();

            foreach (var item in positions) meshGeometry.Positions.Add(item);
            foreach (var item in trianglesIndices) meshGeometry.TriangleIndices.Add(item);
        }
    }
}
