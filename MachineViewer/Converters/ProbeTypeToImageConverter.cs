using MachineViewer.Enums;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Markup;

namespace MachineViewer.Converters
{
    [ContentProperty("Values")]
    public class ProbeTypeToImageConverter : IValueConverter
    {
        public List<ProbeTypeToImageConverterItem> Values { get; set; } = new List<ProbeTypeToImageConverterItem>();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            object result = null;

            if(value is ProbeType pt)
            {
                foreach (var item in Values)
                {
                    if(item.When == pt)
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
    public class ProbeTypeToImageConverterItem
    {
        public ProbeType When { get; set; }
        public object Then { get; set; }
    }
}
