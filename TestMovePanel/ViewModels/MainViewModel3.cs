using GalaSoft.MvvmLight;
using HelixToolkit.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace TestMovePanel.ViewModels
{
    public class MainViewModel3 : ViewModelBase
    {
        private readonly IHelixViewport3D _viewport;

        private double _offsetX;
        public double OffsetX
        {
            get { return _offsetX; }
            set
            {
                if (Set(ref _offsetX, value, nameof(OffsetX)))
                {
                    _traslateTrasform.OffsetX = _offsetX;
                    ProcessChangedBounds();
                }
            }
        }

        private double _offsetZ;
        public double OffsetZ
        {
            get { return _offsetZ; }
            set
            {
                if (Set(ref _offsetZ, value, nameof(OffsetZ)))
                {
                    _traslateTrasform.OffsetZ = _offsetZ;
                    ProcessChangedBounds();
                }
            }
        }

        private double _offsetY;
        public double OffsetY
        {
            get { return _offsetY; }
            set
            {
                if (Set(ref _offsetY, value, nameof(OffsetY)))
                {
                    _traslateTrasform.OffsetY = _offsetY;
                    ProcessChangedBounds();
                }
            }
        }

        private TranslateTransform3D _traslateTrasform;

        public TranslateTransform3D TraslateTrasform
        {
            get => _traslateTrasform; 
            set => Set(ref _traslateTrasform, value, nameof(TraslateTrasform));
        }

        public GeneralTransform3D InversTraslateTrasform { get; set; }

        private Rect3D _sphereBound;
        public Rect3D SphereBound
        {
            get => _sphereBound;
            set => Set(ref _sphereBound, value, nameof(SphereBound));
        }

        private Point3D _center;

        public Point3D Center
        {
            get { return _center; }
            set { Set(ref _center, value, nameof(Center)); }
        }

        public SphereVisual3D Ball { get; set; }

        public BoxVisual3D Box1 { get; set; }

        public Point3DCollection Points { get; set; }

        private bool _boudsIntercept;
        public bool BoudsIntercept
        {
            get => _boudsIntercept;
            set => Set(ref _boudsIntercept, value, nameof(BoudsIntercept));
        }

        private bool _pointsIntercept;

        public bool PointsIntercept
        {
            get => _pointsIntercept; 
            set => Set(ref _pointsIntercept, value, nameof(PointsIntercept));
        }


        public MainViewModel3(IHelixViewport3D viewPort)
        {
            _viewport = viewPort;

            _traslateTrasform = new TranslateTransform3D();
        }

        private void ProcessChangedBounds()
        {
            SphereBound = _traslateTrasform.TransformBounds(Ball.Model.Bounds);
            BoudsIntercept = Box1.Model.Bounds.IntersectsWith(SphereBound);

            if (BoudsIntercept)
            {
                var interceptBox = Rect3D.Intersect(Box1.Model.Bounds, SphereBound);

                var tasks = new List<Task<bool>>();
                var points = Points;

                for (int i = 0; i < points.Count; i++)
                {
                    var m = _traslateTrasform.Value;
                    var p = points[i];
                    var r = Box1.Model.Bounds;

                    tasks.Add(Task.Run(() =>
                    {
                        var pp = m.Transform(p);
                        return r.Contains(pp);
                    }));
                }

                PointsIntercept = Task.WhenAll(tasks).ContinueWith((t) =>
                {
                    return t.Result.Any(b => b);
                }).Result;
            }
            else
            {
                PointsIntercept = false;
            }
        }
    }
}
