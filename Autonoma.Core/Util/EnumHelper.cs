using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Autonoma.Core.Util
{
    public static class EnumHelper
    {
        private static Dictionary<Type, List<EnumValue>> _cache = new Dictionary<Type, List<EnumValue>>();

        public static int GetIntegerValue(this Enum enumValue)
        {
            return Convert.ToInt32(enumValue);
        }

        public static bool HasFlag(this Enum enumValue, int flag)
        {
            return (enumValue.GetIntegerValue() & flag) == flag;
        }

        public static string GetDescription(this Enum enumValue)
        {
            var description = enumValue.GetType().GetMember(enumValue.ToString())
                .Select(x => x.GetCustomAttributes(typeof(DescriptionAttribute), true).FirstOrDefault())
                .FirstOrDefault() as DescriptionAttribute;
            return description == null ? enumValue.ToString() : description.Description;
        }

        public static IDictionary<int, string> ToDictionary(this Enum enumValue)
        {
            return Enum.GetValues(enumValue.GetType()).OfType<Enum>()
                .ToDictionary(x => x.GetIntegerValue(), x => x.GetDescription());
        }

        public static List<EnumValue> GetEnumValueList<T>() where T : Enum
        {
            if (!_cache.TryGetValue(typeof(T), out var result))
            {
                result = new List<EnumValue>();
                foreach (var enumValue in Enum.GetValues(typeof(T)))
                    result.Add(new EnumValue((int)enumValue, enumValue.ToString() ?? "", GetDescription((T)enumValue)));
                _cache.Add(typeof(T), result);
            }
            return result;
        }
    }

    public class EnumValue
    {
        public int IntValue { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public string? LocalizedDescription { get; set; }

        public EnumValue(int intValue, string name, string? description = null, string? localizedDescription = null)
        {
            IntValue = intValue;
            Name = name;
            Description = description;
            LocalizedDescription = localizedDescription;
        }
    }
}
