using System;

namespace Autonoma.UI.Presentation.Abstractions
{
    public interface IElementPrototype
    {
        /// <summary>
        /// Наименование элемента
        /// </summary>
        string? Title { get; set; }

        /// <summary>
        /// Полное имя типа элемента
        /// </summary>
        string? AssemblyQualifiedTypeName { get; set; }

        /// <summary>
        /// Клонируемый экземпляр элемента
        /// </summary>
        IElement? Template { get; set; }

        /// <summary>
        /// Отображаемый экземпляр элемента
        /// </summary>
        IElement? Preview { get; set; }
    }
}