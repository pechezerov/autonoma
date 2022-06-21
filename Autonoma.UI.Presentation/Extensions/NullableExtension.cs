using Avalonia.Markup.Xaml;
using System;

namespace Autonoma.UI.Presentation.Extensions
{
    public sealed class NullableExtension : MarkupExtension
    {
        public NullableExtension(Type underlyingType)
        {
            if (!underlyingType.IsValueType)
                throw new ArgumentException("Underlying type must be value type", nameof(underlyingType));

            NullableType = typeof(Nullable<>).MakeGenericType(underlyingType);
        }

        public Type NullableType { get; }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return NullableType;
        }
    }
}
