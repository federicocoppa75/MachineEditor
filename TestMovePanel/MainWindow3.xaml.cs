using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TestMovePanel.ViewModels;

namespace TestMovePanel
{
    /// <summary>
    /// Logica di interazione per MainWindow.xaml
    /// </summary>
    public partial class MainWindow3 : Window
    {
        MainViewModel3 _model;

        public MainWindow3()
        {
            InitializeComponent();

            _model = new MainViewModel3(view1);
            
            DataContext = _model;
            _model.Ball = ball;
            _model.Box1 = box1;
            _model.Points = (ball.Model.Geometry as System.Windows.Media.Media3D.MeshGeometry3D).Positions;
        }
    }
}
