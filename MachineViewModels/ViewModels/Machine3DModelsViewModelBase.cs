using GalaSoft.MvvmLight;
using HelixToolkit.Wpf;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Windows.Media.Media3D;
using GalaSoft.MvvmLight.CommandWpf;
using EXP = MachineViewModels.Exporters;

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

        private ICommand _exportViewCommand;
        public ICommand ExportViewCommand { get { return _exportViewCommand ?? (_exportViewCommand = new RelayCommand(() => ExportViewCommandImplementation())); } }

        private void ExportViewCommandImplementation()
        {
            var dlg = new Microsoft.Win32.SaveFileDialog();

            dlg.DefaultExt = "stl";
            dlg.AddExtension = true;
            dlg.Filter = "STL file format|*.stl";

            var b = dlg.ShowDialog();

            if (b.HasValue && b.Value)
            {
                foreach (var item in _viewport.Viewport.Children)
                {
                    if (item is MeshGeometryVisual3D mg3D)
                    {
                        EXP.StlExporter.Export(dlg.FileName, mg3D);
                        break;
                    }
                }

            }
        }
    }
}
