using Autonoma.UI.Configuration.Abstractions;
using Autonoma.UI.Configuration.ViewModels;
using System;
using System.Collections.Generic;

namespace Autonoma.UI.Configuration.Design
{
    internal class DesignTopologyProjectViewModel : ITopologyProject
    {
        private static int _counter = 0;
        private static int _maxDepth = 3;

        private static IModelElement GenerateDesignModelViewModel(int depth = 1)
        {
            var result = new ModelElementViewModel
            {
                Name = $"Element{_counter++}"
            };

            if (depth < _maxDepth)
            {
                result.AddElement(GenerateDesignModelViewModel(depth + 1));
                result.AddElement(GenerateDesignModelViewModel(depth + 1));
                result.AddElement(GenerateDesignModelViewModel(depth + 1));
                result.AddElement(GenerateDesignModelViewModel(depth + 1));
            }

            return result;
        }

        public IEnumerable<IModelElement> Elements { get; set; } = new List<IModelElement>()
        {
            GenerateDesignModelViewModel(),
            GenerateDesignModelViewModel(),
            GenerateDesignModelViewModel(),
            GenerateDesignModelViewModel(),
            GenerateDesignModelViewModel(),
        };

        public string Name => "Test";
    }
}