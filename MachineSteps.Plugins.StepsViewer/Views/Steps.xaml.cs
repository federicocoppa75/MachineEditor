using MachineSteps.Plugins.StepsViewer.ViewModels;
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

namespace MachineSteps.Plugins.StepsViewer.Views
{
    /// <summary>
    /// Logica di interazione per Steps.xaml
    /// </summary>
    public partial class Steps : UserControl
    {
        public Steps()
        {
            InitializeComponent();
            DataContext = new StepsViewModel();
        }

        private void OnListViewSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            listView.ScrollIntoView(listView.SelectedItem);            
        }
    }
}
