using Autonoma.UI.Presentation.ViewModels;
using Avalonia.Data.Converters;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace Autonoma.UI.Presentation.Converters
{
    public class ObjectToPropertiesConverter : IValueConverter
    {
        public readonly static ObjectToPropertiesConverter Instance = new();

        private static PropertyViewModelBase? CreatePropertyViewModel(object basicObject, PropertyInfo property)
        {
            Type propertyViewModelGenericType = typeof(PropertyViewModel<>);
            Type[] propertyTypeArg = { property.PropertyType };
            Type propertyViewModelType = propertyViewModelGenericType.MakeGenericType(propertyTypeArg);
            var propertyViewModel = Activator.CreateInstance(propertyViewModelType, new object[] { property, basicObject }) as PropertyViewModelBase;
            return propertyViewModel;
        }

        public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            var result = new List<PropertyViewModelBase>();

            if (value != null)
            {
                var basicObject = value as ViewModelBase;
                if (basicObject != null)
                {
                    var properties = basicObject
                        .GetType().GetProperties().OrderBy(o => o.Name);
                    foreach (var property in properties)
                    {
                        if (!property.CanWrite && !(property.PropertyType.IsGenericType
                            && typeof(IEnumerable).IsAssignableFrom(property.PropertyType))
                            || property.GetCustomAttribute<BrowsableAttribute>()?.Browsable == false)
                            continue;

                        var isReadOnly = property.GetCustomAttribute<ReadOnlyAttribute>()?.IsReadOnly == true;
                        var propertyViewModel = CreatePropertyViewModel(basicObject, property);
                        if (propertyViewModel != null)
                        {
                            propertyViewModel.IsReadOnly = isReadOnly;
                            result.Add(propertyViewModel);
                        }
                    }

                    if (basicObject is ElementViewModel basicElement)
                    {
                        var controlObject = basicElement.Content;
                        if (controlObject != null)
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

                                var propertyViewModel = CreatePropertyViewModel(controlObject, controlProperty);
                                if (propertyViewModel != null)
                                    result.Add(propertyViewModel);
                            }
                        }
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
