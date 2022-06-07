using Autonoma.UI.Presentation.Attributes;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using ReactiveUI;
using System;
using System.Reflection;

namespace Autonoma.UI.Configuration.Views.Tools
{
    public partial class NavigatorToolView : UserControl
    {
        public NavigatorToolView()
        {
            this.InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
