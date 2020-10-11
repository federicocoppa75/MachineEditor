using GalaSoft.MvvmLight;
using HelixToolkit.Wpf;
using System;
using System.Collections.ObjectModel;
using System.Windows.Media.Media3D;

namespace ViewModels.MachineViewModels
{
    public class Machine3DModelsViewModelBase : ViewModelBase
    {
        private readonly IHelixViewport3D _viewport;

        public ObservableCollection<Visual3D> Models { get; set; } = new ObservableCollection<Visual3D>();

        public Machine3DModelsViewModelBase(IHelixViewport3D viewport)
        {
            _viewport = viewport;
        }

        protected void ResetCamera() => (_viewport as HelixViewport3D).FitView(new Vector3D(1, 1, -1), new Vector3D(0, 0, 1), 500);

        public double TraslationStep => (_viewport != null) ? Math.Sqrt(_viewport.Camera.Position.DistanceToSquared(new Point3D())) / 20000.0 : 1.0;

    }
}
