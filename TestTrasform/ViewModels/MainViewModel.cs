using GalaSoft.MvvmLight;
using HelixToolkit.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace TestTrasform.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private readonly IHelixViewport3D _viewport;

        private TranslateTransform3D _traslateTrasform;

        private AxisAngleRotation3D _angleRotation;

        private Material _material;

        public ObservableCollection<Visual3D> Models { get; set; } = new ObservableCollection<Visual3D>() { new SunLight() };

        private double _angle;

        public double Angle
        {
            get { return _angle; }
            set
            {
                Set(ref _angle, value, nameof(Angle));
                _angleRotation.Angle = _angle;
            }
        }

        private double _offsetX;

        public double OffsetX
        {
            get { return _offsetX; }
            set
            {
                Set(ref _offsetX, value, nameof(OffsetX));
                _traslateTrasform.OffsetX = _offsetX;
            }
        }

        private double _offsetY;

        public double OffsetY
        {
            get { return _offsetY; }
            set
            {
                Set(ref _offsetY, value, nameof(OffsetY));
                _traslateTrasform.OffsetY = _offsetY;
            }
        }

        private double _offsetZ;

        public double OffsetZ
        {
            get { return _offsetZ; }
            set
            {
                Set(ref _offsetZ, value, nameof(OffsetZ));
                _traslateTrasform.OffsetZ = _offsetZ;
            }
        }

        private double _opacity = 1.0;

        public double Opacity
        {
            get { return _opacity; }
            set
            {
                if (Set(ref _opacity, value, nameof(Opacity)))
                {
                    MaterialHelper.ChangeOpacity(_material, _opacity);
                }
            }
        }


        public MainViewModel(IHelixViewport3D viewport)
        {
            _viewport = viewport;

            var tt = new TranslateTransform3D();
            //var tt2 = new TranslateTransform3D();
            var ar = new AxisAngleRotation3D(new Vector3D(0, 0, 1), 0.0);
            var tr = new RotateTransform3D(ar);
            var t = new Transform3DGroup();
            //var m = HelixToolkit.Wpf.Materials.Orange.Clone();
            //var m = MaterialHelper.CreateMaterial(new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.DarkCyan), _opacity, 255, false);
            var m = MaterialHelper.CreateMaterial(System.Windows.Media.Colors.DarkCyan, _opacity).Clone();

            

            t.Children.Add(tr);
            t.Children.Add(tt);

            Models.Add(new SphereVisual3D() { Radius = 1 });
            Models.Add(new CubeVisual3D()
            {
                SideLength = 5,
                Transform = t,
                Material = m,
                BackMaterial = m
            });

            _traslateTrasform = tt;
            _angleRotation = ar;
            _material = m;
        }
    }
}
