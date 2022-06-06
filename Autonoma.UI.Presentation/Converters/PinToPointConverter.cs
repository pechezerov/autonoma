﻿using Autonoma.UI.Presentation.Abstractions;
using Avalonia;
using Avalonia.Data.Converters;
using System;
using System.Globalization;

namespace Autonoma.UI.Presentation.Converters
{
    public class PinToPointConverter : IValueConverter
    {
        public readonly static PinToPointConverter Instance = new();

        public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is IPin pin)
            {
                var x = pin.X;
                var y = pin.Y;

                if (pin.Parent is { })
                {
                    x += pin.Parent.X;
                    y += pin.Parent.Y;
                }

                return new Point(x, y);
            }

            return new Point();
        }

        public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
