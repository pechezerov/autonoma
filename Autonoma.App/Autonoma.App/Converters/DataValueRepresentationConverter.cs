using Autonoma.Core;
using Autonoma.Domain;
using System;
using System.Globalization;
using Xamarin.Forms;

namespace Autonoma.App.Converters
{ 
    public class DataValueRepresentationConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is DataValue dv)
            {
                string format = "F3";
                if (parameter != null)
                    format = parameter.ToString();

                if (dv.ValueType == typeof(double))
                    return dv.ValueDouble.ToString(format);
                else if (dv.ValueType == typeof(float))
                    return dv.ValueDouble.ToString(format);
                else
                    return dv.Value.ToString();


            }
            return "n/a";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
