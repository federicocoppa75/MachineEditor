using MachineSteps.Plugins.StepsViewer.Enums;
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
    public class StepStateToImageConverter : IValueConverter
    {
        public List<StepStateToImageConverterItem> Values { get; set; } = new List<StepStateToImageConverterItem>();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            object result = null;

            if (value is StepState pt)
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
    public class StepStateToImageConverterItem
    {
        public StepState When { get; set; }
        public object Then { get; set; }
    }
}
