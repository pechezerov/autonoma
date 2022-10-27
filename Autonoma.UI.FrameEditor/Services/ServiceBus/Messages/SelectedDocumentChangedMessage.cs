using Autonoma.UI.FrameEditor.ViewModels.Documents;

namespace Autonoma.UI.FrameEditor.Services.ServiceBus.Messages
{
    internal class SelectedDocumentChangedMessage
    {
        public SelectedDocumentChangedMessage(DocumentViewModel? currentDocument)
        {
            CurrentDocument = currentDocument;
        }

        public DocumentViewModel? CurrentDocument { get; }
    }
}
