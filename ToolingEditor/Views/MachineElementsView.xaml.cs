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

namespace ToolingEditor.Views
{
    /// <summary>
    /// Logica di interazione per MachineElementsView.xaml
    /// </summary>
    public partial class MachineElementsView : UserControl
    {
        public MachineElementsView()
        {
            InitializeComponent();
        }

        private void treeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            var vm = DataContext as ViewModels.MachineElements.MachineElementsViewModel;
            var me = e.NewValue as ViewModels.MachineElements.MachineElementViewModel;

            if((vm != null) && (me != null))
            {
                vm.Selected = me;
            }
        }

        private void TreeViewItem_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            //if (_waitForTreeviewSelectionChanged)
            //{
            //    _waitForTreeviewSelectionChanged = false;
            //}
            //else
            //{
            //    var item = (sender as System.Windows.Controls.TreeViewItem);
            //    if (item.IsSelected) item.IsSelected = false;
            //}

            //e.Handled = true;
        }

        private void TreeViewItem_Selected(object sender, RoutedEventArgs e)
        {
            //_waitForTreeviewSelectionChanged = true;
            //e.Handled = true;
        }
    }
}
