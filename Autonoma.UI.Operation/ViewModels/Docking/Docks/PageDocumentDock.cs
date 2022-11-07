﻿#nullable enable
using Autonoma.UI.Operation.ViewModels.Docking.Documents;
using Dock.Model.ReactiveUI.Controls;
using ReactiveUI;

namespace Autonoma.UI.Operation.ViewModels.Docking.Docks;

public class PageDocumentDock : DocumentDock
{
    public PageDocumentDock()
    {
        CreateDocument = ReactiveCommand.Create(CreatePage);
    }

    private void CreatePage()
    {
        if (!CanCreateDocument)
        {
            return;
        }

        var page = new PageViewModel()
        {
            Id = "PageDocument",
            Title = "Page"
        };

        Factory?.AddDockable(this, page);
        Factory?.SetActiveDockable(page);
        Factory?.SetFocusedDockable(this, page);
    }
}