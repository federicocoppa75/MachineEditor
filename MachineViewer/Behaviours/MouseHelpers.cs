using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace MachineViewer.Behaviours
{
    public static class MouseHelpers
    {
        #region MouseRightButtonUp

        public static ICommand GetMouseRightButtonUp(DependencyObject obj)
        {
            return (ICommand)obj.GetValue(MouseRightButtonUpProperty);
        }

        public static void SetMouseRightButtonUp(DependencyObject obj, ICommand value)
        {
            obj.SetValue(MouseRightButtonUpProperty, value);
        }

        public static readonly DependencyProperty MouseRightButtonUpProperty =
            DependencyProperty.RegisterAttached("MouseRightButtonUp",
            typeof(ICommand),
            typeof(MouseHelpers),
            new PropertyMetadata(null, new PropertyChangedCallback(MouseRightButtonUpEnter)));

        private static void MouseRightButtonUpEnter(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var element = d as Control;

            if (element != null) element.MouseRightButtonUp += Element_MouseRightButtonUp;
        }

        private static void Element_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            var element = sender as FrameworkElement;
            var command = GetMouseRightButtonUp(element);

            if (command != null && command.CanExecute(null))
            {
                command.Execute(null);
            }
        }

        #endregion
    }
}
