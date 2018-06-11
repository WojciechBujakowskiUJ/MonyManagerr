using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace ClientApp.Helpers.Converters
{
    public class ValueToBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is decimal)
            {
                decimal t = (decimal)value;
                if (t > 0M)
                {
                    return Brushes.DarkGreen;
                }
                else if (t < 0M)
                {
                    return Brushes.Red;
                }
                else
                {
                    return Brushes.DarkGray;
                }
            }
            else
            {
                return Brushes.Black;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Brush)
            {
                Brush t = (Brush)value;
                return t.ToString();
            }
            else
            {
                return null;
            }
        }
    }
}
