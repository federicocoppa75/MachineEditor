using MachineSteps.Plugins.IsoParser;
using Microsoft.Win32;
using System.Windows;

namespace TestIsoParser
{
    /// <summary>
    /// Logica di interazione per MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new OpenFileDialog() { DefaultExt = "iso", Filter ="Cnc iso file |*.iso"  };
            var b = dlg.ShowDialog();

            if (b.HasValue && b.Value)
            {
                IsoParser.Parse(dlg.FileName, true);
            }
        }
    }
}
