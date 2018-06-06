using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace ClientApp.Helpers.Converters
{
    public class HexToColorNameConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string)
            {
                string t = (string)value;
                return ColorsDictionary.GetNameFromHex(t);
            }
            else
            {
                return null;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string)
            {
                string t = (string)value;
                return ColorsDictionary.GetDictionary()[t].ToString();
            }
            else
            {
                return null;
            }
        }
    }
}
