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
    public class NoneToAllConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string t = value as string;
            if (t != null && t == "None")
            {
                return "All";
            }
            else
            {
                return value;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string t = value as string;
            if (t != null && t == "All")
            {
                return "None";
            }
            else
            {
                return value;
            }
        }
    }
}
