﻿using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Autonoma.UI.FrameEditor.Views.Tools
{
    public partial class PropertiesToolView : UserControl
    {
        public PropertiesToolView()
        {
            this.InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
