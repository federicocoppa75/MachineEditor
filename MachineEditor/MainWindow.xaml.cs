using MachineEditor.ViewModels;
using System.Windows;
using System.Windows.Input;

namespace MachineEditor
{
    /// <summary>
    /// Logica di interazione per MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MainViewModel _model;

        public MainWindow()
        {
            InitializeComponent();

            _model = new MainViewModel(view1);
            DataContext = _model;
        }

        private void TreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            _model.Selected = e.NewValue as MachineElementViewModel;
        }

        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.NumPad6:   if (_model.TraslateXPosCommand.CanExecute(null)) _model.TraslateXPosCommand.Execute(_model.TraslationStep);  break;
                case Key.NumPad4:   if (_model.TraslateXPosCommand.CanExecute(null)) _model.TraslateXNegCommand.Execute(_model.TraslationStep); break;
                case Key.NumPad8:   if (_model.TraslateXPosCommand.CanExecute(null)) _model.TraslateYPosCommand.Execute(_model.TraslationStep); break;
                case Key.NumPad2:   if (_model.TraslateXPosCommand.CanExecute(null)) _model.TraslateYNegCommand.Execute(_model.TraslationStep); break;
                case Key.NumPad9:   if (_model.TraslateXPosCommand.CanExecute(null)) _model.TraslateZPosCommand.Execute(_model.TraslationStep); break;
                case Key.NumPad3:   if (_model.TraslateXPosCommand.CanExecute(null)) _model.TraslateZNegCommand.Execute(_model.TraslationStep); break;
                default:    break;
            }
        }
    }
}
