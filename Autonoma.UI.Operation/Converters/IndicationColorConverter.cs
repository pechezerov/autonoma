using Autonoma.Domain;
using Avalonia.Data.Converters;
using Avalonia.Media;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace Autonoma.UI.Operation.Converters
{
    public class IndicationColorConverter : IMultiValueConverter
    {
        public object? Convert(IList<object?> values, Type targetType, object? parameter, CultureInfo culture)
        {
            if (values == null || values.Count != 2 || values[0] == null || values[1] == null)
                return null;

            var value = values[0]! as DataValue;
            var conversionTable = values[1]! as string;

            return new SolidColorBrush(Colors.LimeGreen);
        }
    }
}
