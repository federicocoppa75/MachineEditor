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
    public class MainViewModel : ViewModelBase
    {
        private readonly IHelixViewport3D _viewport;

        private double _offsetX;
        public double OffsetX
        {
            get { return _offsetX; }
            set
            {
                if(Set(ref _offsetX, value, nameof(OffsetX)))
                {
                    _traslateTrasform.OffsetX = _offsetX;
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
                }
            }
        }

        private double _offsetX2;
        public double OffsetX2
        {
            get { return _offsetX2; }
            set
            {
                if (Set(ref _offsetX2, value, nameof(OffsetX2)))
                {
                    _traslateTrasform2.OffsetX = _offsetX2;
                }
            }
        }

        private TranslateTransform3D _traslateTrasform;

        public TranslateTransform3D TraslateTrasform
        {
            get { return _traslateTrasform; }
            set { Set(ref _traslateTrasform, value, nameof(TraslateTrasform)); }
        }

        private TranslateTransform3D _traslateTrasform2;

        public TranslateTransform3D TraslateTrasform2
        {
            get { return _traslateTrasform2; }
            set { Set(ref _traslateTrasform2, value, nameof(TraslateTrasform2)); }
        }


        public ObservableCollection<Visual3D> Models { get; set; } = new ObservableCollection<Visual3D>() { new SunLight() };

        public MainViewModel(IHelixViewport3D viewPort)
        {
            _viewport = viewPort;

            _traslateTrasform = new TranslateTransform3D();
            _traslateTrasform2 = new TranslateTransform3D();
        }
    }
}
