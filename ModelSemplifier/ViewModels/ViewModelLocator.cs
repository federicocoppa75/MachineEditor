using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HelixToolkit.Wpf;
using CommonServiceLocator;

namespace ModelSemplifier.ViewModels
{
    public class ViewModelLocator
    {
        public bool IsInDesignMode => ViewModelBase.IsInDesignModeStatic;

        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            SimpleIoc.Default.Register<MainViewModel>();
            SimpleIoc.Default.Register<ViewportViewModel>();
        }

        public MainViewModel Main => IsInDesignMode ? DummyMainViewModel : SimpleIoc.Default.GetInstance<MainViewModel>();
        public ViewportViewModel ViewPort => IsInDesignMode ? DummyViewPortViewModel : SimpleIoc.Default.GetInstance<ViewportViewModel>();

        public MainViewModel DummyMainViewModel
        {
            get
            {
                var builder = new MeshBuilder();

                builder.AddBox(new System.Windows.Media.Media3D.Point3D(), 1.0, 1.0, 1.0);

                var mesh = builder.ToMesh();

                return new MainViewModel()
                {
                    Viewport1 = new ViewportViewModel() { MeshGeometry = mesh },
                    Viewport2 = new ViewportViewModel() { MeshGeometry = mesh },
                    MeshGeometry1 = mesh,
                    MeshGeometry2 = mesh
                };
            }
        }

        public ViewportViewModel DummyViewPortViewModel
        {
            get
            {
                return new ViewportViewModel();
            }
        }
    }
}
