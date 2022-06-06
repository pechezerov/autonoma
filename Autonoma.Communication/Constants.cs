using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Autonoma.Communication
{
    internal class Constants
    {
        /// <summary>
        /// Значения, относящиеся к внешним API-сервисам
        /// </summary>
        internal static class RemoteServices
        {
            /// <summary>
            /// Название секции конфигурации с настройками сервера авторизации для получения токена доступа
            /// </summary>
            public const string AuthConfigSectionName = "RemoteServices:Auth";

            /// <summary>
            /// Название секции конфигурации с настройками доступа к основному API
            /// </summary>
            public const string AutonomaApiConfigSectionName = "RemoteServices:Autonoma";
        }
    }
}
