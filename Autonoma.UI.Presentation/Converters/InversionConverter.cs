using Avalonia.Data.Converters;
using System;
using System.Globalization;

namespace Autonoma.UI.Presentation.Converters
{
    public class InversionConverter : IValueConverter
    {
        public readonly static InversionConverter Instance = new();

        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value == null) 
                return null;

            if (value is bool bi)
                return !bi;
            if (value is int vi)
                return -vi;
            else if (value is double di)
                return -di;
            else
            {
                try
                {
                    return -System.Convert.ToDouble(value);
                }
                catch
                {
                    return 0;
                }
            }
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
