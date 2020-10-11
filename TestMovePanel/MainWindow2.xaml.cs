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
    public partial class MainWindow2 : Window
    {
        MainViewModel2 _model;

        public MainWindow2()
        {
            InitializeComponent();

            _model = new MainViewModel2(view1);
            
            DataContext = _model;
        }
    }
}
