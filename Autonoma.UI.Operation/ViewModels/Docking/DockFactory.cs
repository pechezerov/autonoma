#nullable enable
using Autonoma.UI.Operation.ViewModels.Docking.Docks;
using Core2D.ViewModels.Docking.Views;
using Core2D.ViewModels.Editor;
using Dock.Avalonia.Controls;
using Dock.Model.Controls;
using Dock.Model.Core;
using Dock.Model.ReactiveUI;
using Dock.Model.ReactiveUI.Controls;
using System;
using System.Collections.Generic;

namespace Autonoma.UI.Operation.ViewModels.Docking;

public class DockFactory : Factory
{
    private readonly ProjectEditorViewModel _projectEditor;
    private IRootDock? _rootDock;
    private IDocumentDock? _pagesDock;
    private IProportionalDock? _homeDock;

    public IRootDock? RootDock => _rootDock;

    public IDocumentDock? PagesDock => _pagesDock;

    public IProportionalDock? HomeDock => _homeDock;

    public DockFactory(ProjectEditorViewModel projectEditor)
    {
        _projectEditor = projectEditor;
    }

    public override IDocumentDock CreateDocumentDock()
    {
        return new PageDocumentDock();
    }

    public override IRootDock CreateLayout()
    {
        // Home Perspective

        var documentDock = new PageDocumentDock
        {
            Id = "PageDocumentDock",
            Title = "Pages",
            ActiveDockable = null,
            VisibleDockables = CreateList<IDockable>(),
            CanCreateDocument = true,
            IsCollapsable = false
        };

        var homeDock = new ProportionalDock
        {
            Id = "HomeDock",
            Orientation = Orientation.Horizontal,
            ActiveDockable = documentDock,
            VisibleDockables = CreateList<IDockable>
            (
                documentDock
            ),
            IsCollapsable = false
        };

        var homeMenuViewModel = new HomeMenuViewModel()
        {
            Id = "HomeMenuView",
            Dock = DockMode.Top
        };

        var homeStatusBarViewModel = new HomeStatusBarViewModel()
        {
            Id = "HomeStatusBarView",
            Dock = DockMode.Bottom
        };

        var homeViewModel = new HomeViewModel
        {
            Id = "HomeView",
            Dock = DockMode.Center,
            ActiveDockable = homeDock,
            VisibleDockables = CreateList<IDockable>(homeDock),
            IsCollapsable = false
        };

        var homeDockDock = new DockDock()
        {
            Id = "HomeDockDock",
            LastChildFill = true,
            VisibleDockables = CreateList<IDockable>
            (
                homeMenuViewModel,
                homeStatusBarViewModel,
                homeViewModel
            ),
            IsCollapsable = false
        };

        // Dashboard Perspective

        var dashboardMenuViewModel = new DashboardMenuViewModel()
        {
            Id = "DashboardMenuView",
            Title = "Dashboard Menu",
            Dock = DockMode.Top,
            IsCollapsable = false
        };

        var dashboardViewModel = new DashboardViewModel
        {
            Id = "DashboardView",
            Title = "Dashboard",
            Dock = DockMode.Center,
            IsCollapsable = false
        };

        var dashboardDockDock = new DockDock()
        {
            Id = "DashboardDock",
            Proportion = 1.0,
            LastChildFill = true,
            VisibleDockables = CreateList<IDockable>
            (
                dashboardMenuViewModel,
                dashboardViewModel
            ),
            IsCollapsable = false
        };

        // Root Perspective

        var dashboardRootDock = CreateRootDock();
        dashboardRootDock.Id = "Dashboard";
        dashboardRootDock.ActiveDockable = dashboardDockDock;
        dashboardRootDock.DefaultDockable = dashboardDockDock;
        dashboardRootDock.VisibleDockables = CreateList<IDockable>(dashboardDockDock);
        dashboardRootDock.IsCollapsable = false;

        var homeRootDock = CreateRootDock();
        homeRootDock.Id = "Home";
        homeRootDock.ActiveDockable = homeDockDock;
        homeRootDock.DefaultDockable = homeDockDock;
        homeRootDock.VisibleDockables = CreateList<IDockable>(homeDockDock);
        homeRootDock.IsCollapsable = false;

        // Root Dock

        var rootDock = CreateRootDock();
        rootDock.Id = "Root";
        rootDock.ActiveDockable = dashboardRootDock;
        rootDock.DefaultDockable = dashboardRootDock;
        rootDock.VisibleDockables = CreateList<IDockable>(dashboardRootDock, homeRootDock);
        rootDock.IsCollapsable = false;

        _rootDock = rootDock;
        _pagesDock = documentDock;
        _homeDock = homeDock;

        return rootDock;
    }

    public override void InitLayout(IDockable layout)
    {
        ContextLocator = new Dictionary<string, Func<object>>
        {
            // Documents
            ["PageDocument"] = () => _projectEditor,
            ["PageDocumentDock"] = () => _projectEditor,
            // Home
            ["HomeMenuView"] = () => _projectEditor,
            ["HomeView"] = () => _projectEditor,
            ["HomeDock"] = () => _projectEditor,
            ["HomeStatusBarView"] = () => _projectEditor,
            ["HomeDockDock"] = () => _projectEditor,
            // Dashboard
            ["DashboardMenuView"] = () => _projectEditor,
            ["DashboardView"] = () => _projectEditor,
            ["DashboardDock"] = () => _projectEditor,
            ["DashboardDockDock"] = () => _projectEditor,
            // Root
            ["Dashboard"] = () => _projectEditor,
            ["Home"] = () => _projectEditor,
            ["Root"] = () => _projectEditor
        };

        DockableLocator = new Dictionary<string, Func<IDockable?>>
        {
            ["Root"] = () => _rootDock,
            ["Pages"] = () => _pagesDock,
            ["Home"] = () => _homeDock,
        };

        HostWindowLocator = new Dictionary<string, Func<IHostWindow>>
        {
            [nameof(IDockWindow)] = () => new HostWindow()
        };

        base.InitLayout(layout);
    }
}
