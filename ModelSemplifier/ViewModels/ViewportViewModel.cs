using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace ModelSemplifier.ViewModels
{
    public class ViewportViewModel : ViewModelBase
    {
        private PerspectiveCamera _camera;

        public PerspectiveCamera Camera
        {
            get => _camera;
            set => Set(ref _camera, value, nameof(Camera));
        }

        private MeshGeometry3D _meshGeometry;
        public MeshGeometry3D MeshGeometry
        {
            get => _meshGeometry;
            set
            {
                if(Set(ref _meshGeometry, value, nameof(MeshGeometry)))
                {
                    RaisePropertyChanged(nameof(VertexesCount));
                    RaisePropertyChanged(nameof(FacesCount));
                }
            }
        }

        public int VertexesCount => (_meshGeometry != null) ? _meshGeometry.Positions.Count : 0;

        public int FacesCount => (_meshGeometry != null) ? _meshGeometry.TriangleIndices.Count / 3 : 0; 
    }
}
