using Autonoma.UI.Presentation.ViewModels;
using Dock.Model.ReactiveUI.Controls;
using System;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace Autonoma.UI.FrameEditor.ViewModels.Documents
{
    public class DocumentViewModel : Document
    {
        [Reactive]
        public FrameViewModel Frame { get; set; }

        [Reactive]
        public string? FilePath { get; set; }

        public DocumentViewModel(FrameViewModel frame)
        {
            Frame = frame;
            Title = frame.Name;

            Frame.WhenAnyValue(x => x.Name).Subscribe(x => Title = x);
        }
    }
}
