using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Markup;

namespace MachineSteps.Plugins.StepsViewer.Converters
{
    [ContentProperty("Values")]
    public class AxIdToLabelCharConverter : IValueConverter
    {
        public List<AxIdToLabelCharConverterItem> Values { get; set; } = new List<AxIdToLabelCharConverterItem>();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            object result = null;

            if (value is int v)
            {
                foreach (var item in Values)
                {
                    if (item.When == v)
                    {
                        result = item.Then;
                        break;
                    }
                }
            }

            if (result == null) result = value;

            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    [ContentProperty("Then")]
    public class AxIdToLabelCharConverterItem
    {
        public int When { get; set; }
        public object Then { get; set; }
    }
}
