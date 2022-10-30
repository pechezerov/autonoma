using Autonoma.UI.Presentation.Attributes;
using ReactiveUI;
using System;
using System.Reflection;

namespace Autonoma.UI.Presentation.ViewModels
{
    public abstract class PropertyViewModelBase : ReactiveObject
    {
        public string Name { get; }

        public string? Description { get; }

        public abstract Type Type { get; }

        public bool? IsReadOnly { get; set; }

        public PropertyViewModelBase(string name)
        {
            Name = name;
        }
    }

    public class PropertyViewModel<T> : PropertyViewModelBase
    {
        private readonly object _source;
        private readonly PropertyInfo _property;

        public PropertyViewModel(PropertyInfo prop, object source) : base(prop.Name)
        {
            this._property = prop;
            this._source = source;

            IsConnectable = prop.GetCustomAttribute<ConnectedAttribute>() != null;

            Value = (T)prop.GetMethod?.Invoke(source, null);
        }

        public override Type Type => _property.PropertyType;

        public bool IsConnectable { get; }

        private T? _value;
        public T Value
        {
            get
            {
                if (_value == null)
                {
                    var v = _property.GetMethod?.Invoke(_source, null);
                    if (v is T)
                        _value = (T)v;
                    if (typeof(T) == typeof(string))
                        _value = default;
                    else
                        _value = Activator.CreateInstance<T>();
                }
                return _value;
            }
            set
            {
                try
                {
                    _value = (T)Convert.ChangeType(value, _property.PropertyType);
                    _property.SetMethod?.Invoke(_source, new object?[] { _value });
                }
                catch { }
            }
        }

        public Array Values
        {
            get
            {
                if (_property.PropertyType.IsEnum)
                    return Enum.GetValues(_property.PropertyType);
                return Array.Empty<object>();
            }
        }
    }
}
