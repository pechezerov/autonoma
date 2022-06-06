using Autonoma.UI.Presentation.Abstractions;
using Autonoma.UI.Presentation.Attributes;
using Autonoma.UI.Presentation.Controls;
using Autonoma.UI.Presentation.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;

namespace Autonoma.UI.Presentation.Infrastructure
{
    public class ElementFactory : IElementFactory
    {
        public IList<IElementPrototype> CreateToolbox()
        {
            var controlPrototypes = new List<IElementPrototype>();

            var assembly = Assembly.GetAssembly(typeof(LooklessControlViewModel));
            if (assembly == null)
                throw new InvalidOperationException("Не удалось выполнить загрузку панели элементов, т.к. не определена базовая сборка");

            foreach (var controlViewModelType in assembly.GetTypes()
                .Where(t => t.IsAssignableTo(typeof(LooklessControlViewModel)))
                .Where(t => !t.IsAbstract))
            {
                var sample = CreateElement(controlViewModelType);

                controlPrototypes.Add(new ElementPrototype
                {
                    Title = controlViewModelType.Name,
                    AssemblyQualifiedTypeName = controlViewModelType.AssemblyQualifiedName,
                    Preview = sample,
                    Template = sample
                });
            }

            return new ObservableCollection<IElementPrototype>(controlPrototypes);
        }

        public IElement CreateElement(Type elementContentType)
        {
            if (!elementContentType.IsAssignableTo(elementContentType))
                throw new InvalidOperationException("");

            var sampleContent = Activator.CreateInstance(elementContentType) as IElementContent;
            var sample = new ElementViewModel
            {
                Content = sampleContent,
            };

            var pinsMetaData = elementContentType.GetCustomAttributes<PinAttribute>(false);
            foreach (var pinMetaData in pinsMetaData)
                sample.AddPin(pinMetaData);

            var defaultSizeMetadata = elementContentType.GetCustomAttribute<DefaultSizeAttribute>();
            sample.Width = defaultSizeMetadata?.Width ?? 50;
            sample.Height = defaultSizeMetadata?.Height ?? 50;
            return sample;
        }
    }
}
