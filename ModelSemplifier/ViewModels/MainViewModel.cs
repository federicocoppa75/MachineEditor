using g3;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace ModelSemplifier.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private string _title;

        public string Title
        {
            get => _title; 
            set => Set(ref _title, value, nameof(Title));
        }

        public DMesh3 Mesh { get; set; }

        private MeshGeometry3D _meshGeometry1;
        public MeshGeometry3D MeshGeometry1
        {
            get => _meshGeometry1;
            set
            {
                if(Set(ref _meshGeometry1, value, nameof(MeshGeometry1)))
                {
                    Viewport1.MeshGeometry = _meshGeometry1;
                }
            }
        }

        private MeshGeometry3D _meshGeometry2;
        public MeshGeometry3D MeshGeometry2
        {
            get => _meshGeometry2;
            set
            {
                if(Set(ref _meshGeometry2, value, nameof(MeshGeometry2)))
                {
                    Viewport2.MeshGeometry = _meshGeometry2;
                }
            }
        }

        public ViewportViewModel Viewport1 { get; set; }

        public ViewportViewModel Viewport2 { get; set; }

        private int _reduxFactor = 1;
        public int ReduxFactor
        {
            get => _reduxFactor;
            set
            {
                if(Set(ref _reduxFactor, value, nameof(ReduxFactor)))
                {
                    if(_reduxFactor == 1)
                    {
                        MeshGeometry2 = MeshGeometry1;
                    }
                    else
                    {
                        ProcessReduction((m) => MeshGeometry2 = ToMeshGeometry3D(m));
                    }
                }
            }
        }

        private ICommand _fileOpenCommand;
        public ICommand FileOpenCommand => _fileOpenCommand ?? (_fileOpenCommand = new RelayCommand(() => FileOpenCommandImpl()));

        private ICommand _fileSaveCommand;
        public ICommand FileSaveCommand => _fileSaveCommand ?? (_fileSaveCommand = new RelayCommand(() => FileSaveCommandImpl()));

        public MainViewModel()
        {
            Title = "MvvmCameraDemo";
            Viewport1 = new ViewportViewModel();
            Viewport2 = new ViewportViewModel();

            var camera = new PerspectiveCamera()
            {
                Position = new Point3D(0, -10, 0),
                LookDirection = new Vector3D(0, 10, 0),
                UpDirection = new Vector3D(0, 0, 1),
                FieldOfView = 60,
            };

            Viewport1.Camera = camera;
            Viewport2.Camera = camera;
        }

        private void FileSaveCommandImpl()
        {
            var dlg = new Microsoft.Win32.SaveFileDialog() { DefaultExt = "stp", AddExtension = true, Filter = "Mesh file |*.stl" };
            var b = dlg.ShowDialog();

            if (b.HasValue && b.Value)
            {
                if(_reduxFactor == 1)
                {
                    StandardMeshWriter.WriteMesh(dlg.FileName, Mesh, WriteOptions.Defaults);
                }
                else
                {
                    ProcessReduction((m) =>
                    {
                        var options = WriteOptions.Defaults;
                        options.bWriteBinary = true;
                        StandardMeshWriter.WriteMesh(dlg.FileName, m, options);
                    });
                }
            }
        }

        private void FileOpenCommandImpl()
        {
            var dlg = new Microsoft.Win32.OpenFileDialog() { DefaultExt = "stl", AddExtension = true, Filter = "Mesh file |*.stl" };
            var b = dlg.ShowDialog();

            if (b.HasValue && b.Value)
            {
                StandardMeshReader reader = new StandardMeshReader();
                reader.MeshBuilder = new DMesh3Builder();
                reader.Read(dlg.FileName, new ReadOptions());

                var mesh = (reader.MeshBuilder as DMesh3Builder).Meshes[0];
                var mg = ToMeshGeometry3D(mesh);

                Mesh = mesh;
                MeshGeometry1 = mg;
                MeshGeometry2 = mg;
                _reduxFactor = 1;
            }
        }

        private MeshGeometry3D ToMeshGeometry3D(DMesh3 src)
        {
            MeshGeometry3D dest = new MeshGeometry3D();

            if (!src.IsCompact) src.CompactInPlace();

            var vertices = src.VerticesBuffer;

            foreach (int vId in src.VerticesRefCounts)
            {
                int i = vId * 3;
                dest.Positions.Add(new Point3D(vertices[i], vertices[i + 1], vertices[i + 2]));
            }

            var triangles = src.TrianglesBuffer;

            foreach (int tId in src.TrianglesRefCounts)
            {
                int i = tId * 3;
                dest.TriangleIndices.Add(triangles[i]);
                dest.TriangleIndices.Add(triangles[i + 1]);
                dest.TriangleIndices.Add(triangles[i + 2]);
            }

            return dest;
        }


        private void ProcessReduction(Action<DMesh3> resultAction)
        {
            var mesh = new DMesh3(Mesh);
            var tCount = Mesh.TriangleCount / _reduxFactor;
            var resucer = new Reducer(mesh);
            DMeshAABBTree3 tree = new DMeshAABBTree3(new DMesh3(mesh));

            tree.Build();
            MeshProjectionTarget target = new MeshProjectionTarget(tree.Mesh, tree);

            resucer.ReduceToTriangleCount(tCount);
            resucer.SetExternalConstraints(new MeshConstraints());
            resucer.SetProjectionTarget(target);
            resucer.ProjectionMode = Reducer.TargetProjectionMode.Inline;

            MeshConstraintUtil.FixAllBoundaryEdges(resucer.Constraints, mesh);

            resultAction?.Invoke(mesh);
        }
    }
}
