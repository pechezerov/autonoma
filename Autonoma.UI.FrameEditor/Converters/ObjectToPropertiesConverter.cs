using Autonoma.UI.Presentation.Attributes;
using Autonoma.UI.Presentation.Controls;
using Autonoma.UI.Presentation.ViewModels;
using Avalonia.Data.Converters;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Reflection;
using static Autonoma.UI.FrameEditor.Views.Tools.PropertiesToolView;

namespace Autonoma.UI.FrameEditor.Converters
{
    public class ObjectToPropertiesConverter : IValueConverter
    {
        public readonly static ObjectToPropertiesConverter Instance = new();

        public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            var result = new List<PropertyViewModel>();

            if (value != null)
            {
                // Свойства оболочки
                if (value is ElementViewModel wrapperObject)
                {
                    var wrapperProperties = wrapperObject
                        .GetType().GetProperties().OrderBy(o => o.Name);
                    var fixedSize = wrapperObject.GetType().GetCustomAttribute<FixedSizeAttribute>() != null;

                    foreach (var property in wrapperProperties)
                    {
                        if ((property.Name == nameof(ElementViewModel.Width) || property.Name == nameof(ElementViewModel.Height))
                            && fixedSize)
                            continue;

                        if (!property.CanWrite && !(property.PropertyType.IsGenericType
                            && typeof(IEnumerable).IsAssignableFrom(property.PropertyType))
                            || property.GetCustomAttribute<BrowsableAttribute>()?.Browsable == false)
                            continue;

                        var wrapperPropertyModel = new PropertyViewModel(property, wrapperObject);
                        result.Add(wrapperPropertyModel);
                    }

                    // позволяем обработать свойства элемента
                    value = wrapperObject.Content;
                }

                // Свойства элемента
                if (value is LooklessControlViewModel controlObject)
                {
                    var controlProperties = controlObject
                        .GetType().GetProperties()
                        .OrderBy(o => o.Name);
                    foreach (var controlProperty in controlProperties)
                    {
                        if (!controlProperty.CanWrite && !(controlProperty.PropertyType.IsGenericType
                            && typeof(IEnumerable).IsAssignableFrom(controlProperty.PropertyType))
                            || controlProperty.GetCustomAttribute<BrowsableAttribute>()?.Browsable == false)
                            continue;
                        var property = new PropertyViewModel(controlProperty, controlObject);
                        result.Add(property);
                    }
                }
            }

            return result;
        }

        public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
