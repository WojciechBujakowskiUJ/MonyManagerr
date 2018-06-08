﻿using Interfaces;
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
                    return "None";
                }
            }
            else
            {
                return "None";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}