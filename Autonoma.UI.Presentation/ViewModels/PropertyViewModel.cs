using Autonoma.UI.Presentation.Attributes;
using ReactiveUI;
using System;
using System.Reflection;

namespace Autonoma.UI.Presentation.ViewModels
{
    public abstract class PropertyViewModelBase : ReactiveObject
    {
        public string Name { get; }
        public abstract Type Type { get; }

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

            Value = (T?)prop.GetMethod?.Invoke(source, null);
        }

        public override Type Type => _property.PropertyType;

        public bool IsConnectable { get; }

        public T? Value
        {
            get
            {
                var v =  _property.GetMethod?.Invoke(_source, null);
                if (v is T)
                    return (T)v;
                return default(T);
            }
            set
            {
                try
                {
                    _property.SetMethod?.Invoke(_source, new[] { Convert.ChangeType(value, _property.PropertyType) });
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
