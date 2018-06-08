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
    public class ValueToBackgroundBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is decimal)
            {
                decimal t = (decimal)value;
                if (t > 0M)
                {
                    //return new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFE0FFFF"));
                    return Brushes.LightGreen;
                }
                else if (t < 0M)
                {
                    //return new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFE7E0FF"));
                    return Brushes.LightPink;
                }
                else
                {
                    //return new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFEEEEEE"));
                    return Brushes.LightGray;
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
