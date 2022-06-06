using System;

namespace Autonoma.Extensibility.Shared.Contracts
{
    /// <summary>
    /// Класс, предназначенный для передачи произвольных данных.
    /// В текущей реализации поддерживаются три часто используемых типа.
    /// </summary>
    public class PluginAttributeInfo
    {
        public PluginAttributeInfo()
        {

        }

        public PluginAttributeInfo(string name, object value)
        {
            Name = name;

            if (value is string s)
            {
                StringValue = s;
                TypeName = typeof(string).FullName;
            }
            if (value is int i)
            {
                IntValue = i;
                TypeName = typeof(int).FullName;
            }
            if (value is bool b)
            {
                BoolValue = b;
                TypeName = typeof(bool).FullName;
            }
        }

        public PluginAttributeInfo(string name, Type type, string desc = "")
        {
            Name = name;
            TypeName = type.FullName;
            Description = desc;
        }

        public bool BoolValue { get; set; }
        public string Description { get; set; }
        public int IntValue { get; set; }
        public string Name { get; set; }
        public string StringValue { get; set; }
        public string TypeName { get; set; }
    }
}
