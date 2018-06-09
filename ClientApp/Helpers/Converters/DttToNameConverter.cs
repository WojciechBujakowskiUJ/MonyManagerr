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
    public class DttToNameConverter : IValueConverter
    {
        private const string DEFAULT_FALLBACK_STRING = "None";

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is ITransactionType)
            {
                var t = (value as ITransactionType);
                if (t != null)
                {
                    return t.Name;
                }
                else
                {
                    string param = parameter as string;
                    if (param != null)
                    {
                        return param;
                    }
                    else
                    {
                        return DEFAULT_FALLBACK_STRING;
                    }
                }
            }
            else
            {
                return DEFAULT_FALLBACK_STRING;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
