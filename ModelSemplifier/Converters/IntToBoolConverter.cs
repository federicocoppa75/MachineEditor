using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace ModelSemplifier.Converters
{
    public class IntToBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool result = false;
            int v = (int)value;

            if(int.TryParse(parameter.ToString(), out int p))
            {
                result = v == p;
            }

            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int result = 0;
            bool v = (bool)value;

            if (v && int.TryParse(parameter.ToString(), out int p))
            {
                result = p;
            }

            return result;
        }
    }
}
