using GalaSoft.MvvmLight.Messaging;
using MachineViewer.Messages.Struct;
using MachineViewer.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace MachineViewer.Views
{
    /// <summary>
    /// Logica di interazione per Struct.xaml
    /// </summary>
    public partial class Struct : UserControl
    {
        private bool _waitForTreeviewSelectionChanged;

        public Struct()
        {
            InitializeComponent();
        }

        private void TreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            Messenger.Default.Send(new SelectedStructItemChangedMessage() { Value = e.NewValue as MachineElementViewModel });
            e.Handled = true;
        }

        private void TreeViewItem_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (_waitForTreeviewSelectionChanged)
            {
                _waitForTreeviewSelectionChanged = false;
            }
            else
            {
                var item = (sender as System.Windows.Controls.TreeViewItem);
                if (item.IsSelected) item.IsSelected = false;
            }

            e.Handled = true;
        }

        private void TreeViewItem_Selected(object sender, RoutedEventArgs e)
        {
            _waitForTreeviewSelectionChanged = true;
            e.Handled = true;
        }
    }
}
