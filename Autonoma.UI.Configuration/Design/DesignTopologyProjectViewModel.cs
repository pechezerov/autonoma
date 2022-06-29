using Autonoma.Domain.Entities;
using Autonoma.UI.Configuration.Abstractions;
using Autonoma.UI.Configuration.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Autonoma.UI.Configuration.Design
{
    internal class DesignTopologyProjectViewModel : ITopologyProject
    {
        private static int _counter = 0;
        private static int _maxDepth = 3;

        private static IModelElement GenerateDesignModelViewModel(int depth = 1)
        {
            var result = new DesignModelElementViewModel(_counter++);

            if (depth < _maxDepth)
            {
                result.AddElement(GenerateDesignModelViewModel(depth + 1));
                result.AddElement(GenerateDesignModelViewModel(depth + 1));
                result.AddElement(GenerateDesignModelViewModel(depth + 1));
                result.AddElement(GenerateDesignModelViewModel(depth + 1));
            }

            return result;
        }

        public void AddElement(IModelElement element)
        {
            Elements.Add(element);
        }

        public ObservableCollection<IModelElement> Elements { get; set; } = new ObservableCollection<IModelElement>()
        {
            GenerateDesignModelViewModel(),
            GenerateDesignModelViewModel(),
            GenerateDesignModelViewModel(),
            GenerateDesignModelViewModel(),
            GenerateDesignModelViewModel(),
        };
        IList<IModelElement> IModelElement.Elements => Elements;

        public bool AllowEditElements => true;
        public string Name => "Test";

        public IModelElement? Parent { get; set; }

        public IList<IModelAttribute> Attributes => new List<IModelAttribute>
        {
            new ModelAttributeViewModel { Name = "IntProperty", Value = "1" },
            new ModelAttributeViewModel { Name = "StringProperty", Value = "text" },
        };
    }
}