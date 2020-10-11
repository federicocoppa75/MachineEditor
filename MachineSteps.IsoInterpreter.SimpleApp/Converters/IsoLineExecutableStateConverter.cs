using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Markup;

namespace MachineSteps.IsoInterpreter.SimpleApp.Converters
{
    [ContentProperty("Values")]
    public class IsoLineExecutableStateConverter : IValueConverter
    {
        public List<IsoLineExecutableStateConverterItem> Values { get; set; } = new List<IsoLineExecutableStateConverterItem>();
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            object result = null;

            if (value is bool pt)
            {
                foreach (var item in Values)
                {
                    if (item.When == pt)
                    {
                        result = item.Then;
                        break;
                    }
                }
            }

            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    [ContentProperty("Then")]
    public class IsoLineExecutableStateConverterItem
    {
        public bool When { get; set; }
        public object Then { get; set; }
    }
}
