using Autonoma.Domain;
using Autonoma.UI.Presentation.Abstractions;
using Autonoma.UI.Presentation.ViewModels;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using System.Collections.Generic;
using System.ComponentModel;

namespace Autonoma.UI.Presentation.Controls
{
    public partial class LooklessControl : UserControl
    {
        public LooklessControl()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }

    public abstract class LooklessControlViewModel : ViewModelBase, IElementContent
    {
        [Browsable(false)]
        public Dictionary<string, string> Connections { get; set; } = new Dictionary<string, string>();

        public virtual void Update(DataPointInfo update) { }
    }
}
