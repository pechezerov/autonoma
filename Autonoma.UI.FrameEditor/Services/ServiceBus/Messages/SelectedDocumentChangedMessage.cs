using Autonoma.UI.FrameEditor.ViewModels.Documents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
