using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Core2D.ViewModels.Editor;
using System.Linq;

namespace Autonoma.UI.Core2D.Editor.Controls.Editor
{
    public class ToolsView : UserControl
    {
        public ToolsView()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public void OnToolElectricLine()
        {
            if (DataContext is ProjectEditorViewModel ds)
            {
                if (ds.CurrentTool?.Title == "Path" && ds.CurrentPathTool?.Title != "ElectricLine")
                {
                    ds.CurrentPathTool?.Reset();
                    ds.CurrentPathTool = ds.PathTools.FirstOrDefault(t => t.Title == "ElectricLine");
                }
                else
                {
                    ds.OnResetTool();
                    ds.CurrentTool = ds.Tools.FirstOrDefault(t => t.Title == "ElectricLine");
                }
            }
        }

        public void OnToolElectricSwitch()
        {
            if (DataContext is ProjectEditorViewModel ds)
            {
                ds.OnResetTool();
                ds.CurrentTool = ds.Tools.FirstOrDefault(t => t.Title == "ElectricSwitch");
            }
        }
    }
}
