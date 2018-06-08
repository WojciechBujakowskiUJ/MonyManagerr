using Interfaces;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace ClientApp.Helpers.Converters
{
    public class CustomHeightConverter : IValueConverter
    {
        public const double MIN_GRID_PANEL_HEIGHT = 110.0;

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (double.TryParse(value.ToString(), out double height))
            {
                if (double.TryParse(parameter.ToString(), out double paramValue))
                {
                    double result = height - paramValue;
                    return result > MIN_GRID_PANEL_HEIGHT ? result : MIN_GRID_PANEL_HEIGHT;
                }
                else
                {
                    return MIN_GRID_PANEL_HEIGHT;
                }
            }
            return MIN_GRID_PANEL_HEIGHT;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
