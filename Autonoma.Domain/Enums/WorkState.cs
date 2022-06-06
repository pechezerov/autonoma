using System.ComponentModel;

namespace Autonoma.Domain
{
    public enum WorkState
    {
        [Description("Отключен")]
        Off,
        [Description("Ожидание")]
        Pending,
        [Description("В работе")]
        On,
        [Description("Ошибка")]
        Error
    }
}