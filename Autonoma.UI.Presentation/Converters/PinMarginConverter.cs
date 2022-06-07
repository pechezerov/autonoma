using Autonoma.UI.Presentation.Abstractions;
using Avalonia;
using Avalonia.Data.Converters;
using System;
using System.Globalization;

namespace Autonoma.UI.Presentation.Converters
{
    public class PinMarginConverter : IValueConverter
    {
        public readonly static PinMarginConverter Instance = new();

        public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is IPin pin)
            {
                if (pin.Alignment == Model.PinAlignment.Right)
                    return new Thickness(-pin.Width, -pin.Height / 2, 0, 0);
                if (pin.Alignment == Model.PinAlignment.Left)
                    return new Thickness(-pin.Width / 2, -pin.Height / 2, -pin.Width, 0);

                if (pin.Alignment == Model.PinAlignment.Top)
                    return new Thickness(-pin.Width / 2, -pin.Height / 2, 0, -pin.Height);
                if (pin.Alignment == Model.PinAlignment.Bottom)
                    return new Thickness(-pin.Width / 2, -pin.Height, 0, 0);

                return new Thickness(-pin.Width, -pin.Height, 0, 0);
            }

            return new Thickness(0);
        }

        public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
