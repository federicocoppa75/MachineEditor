using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

namespace TestXxlParse
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
            var dlg = new OpenFileDialog() { DefaultExt = "xxl" };
            var b = dlg.ShowDialog();

            if(b.HasValue && b.Value)
            {
                using (var stream = File.OpenText(dlg.FileName))
                {
                    while(true)
                    {
                        var line = stream.ReadLine();

                        if (line == null) break;

                        if (!string.IsNullOrWhiteSpace(line)) ParseLine(line);
                    }
                    
                }
            }
        }

        private void ParseLine(string line)
        {
            var commentRgx = new Regex("^\\s*;.*");
            var labelRgx = new Regex("\\.\\S*");
            
            if(commentRgx.Match(line).Success)
            {

            }
            else if(labelRgx.Match(line).Success)
            {

            }
            else
            {

            }
        }

        private void ParseIstruction(string line)
        {

        }
    }
}
