﻿using Autonoma.UI.Presentation.Attributes;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Autonoma.UI.Presentation.Controls.Electric
{
    public partial class DisconnectorControl : UserControl
    {
        public DisconnectorControl()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }

    [DefaultSize(50, 50)]
    [Pin(0, 25, Model.PinAlignment.Left)]
    [Pin(50, 25, Model.PinAlignment.Right)]
    public class DisconnectorControlViewModel : SwitchControlViewModel
    {
        public DisconnectorControlViewModel()
        {
        }
    }
}
