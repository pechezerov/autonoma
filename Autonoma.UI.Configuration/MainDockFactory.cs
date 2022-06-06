using Autonoma.UI.Configuration.ViewModels;
using Autonoma.UI.Configuration.ViewModels.Tools;
using Autonoma.UI.FrameEditor.ViewModels;
using Avalonia.Controls;
using Avalonia.Data;
using Dock.Avalonia.Controls;
using Dock.Model.Controls;
using Dock.Model.Core;
using Dock.Model.ReactiveUI;
using Dock.Model.ReactiveUI.Controls;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Autonoma.UI.Configuration
{
    public class MainDockFactory : Factory, IMainDockFactory
    {
        public IDocumentDock MainDocumentDock { get; set; } = null!;

        public MainWindowViewModel MainContext { get; set; } = null!;

        public MainDockFactory() { }

        public override IRootDock CreateLayout()
        {
            var navigatorTool = new NavigatorToolViewModel
            {
                Id = "Properties",
                Title = "Properties",
                Context = MainContext
            };
            MainContext.Tools.Add(navigatorTool);

            MainDocumentDock = new DocumentDock
            {
                Id = "DocumentsPane",
                Title = "DocumentsPane",
                Proportion = double.NaN,
                Dock = DockMode.Center,
                IsCollapsable = false,
                CanFloat = false,
                CanCreateDocument = false
            };

            MainDocumentDock.CreateDocument = ReactiveCommand.Create(() =>
            {
                var project = ProjectFactory.CreateEmptyProject("Новый проект");
                LoadProject(project);
            });

            var mainLayout = new ProportionalDock
            {
                Id = "MainLayout",
                Title = "MainLayout",
                Proportion = double.NaN,
                Orientation = Orientation.Horizontal,
                ActiveDockable = null,
                VisibleDockables = CreateList<IDockable>
                (
                    new ProportionalDock
                    {
                        Id = "LeftPane",
                        Title = "LeftPane",
                        Proportion = 0.2,
                        Orientation = Orientation.Vertical,
                        ActiveDockable = null,
                        VisibleDockables = CreateList<IDockable>
                        (
                            new ToolDock
                            {
                                Id = "LeftPaneTop",
                                Title = "LeftPaneTop",
                                Proportion = 0.4,
                                ActiveDockable = navigatorTool,
                                VisibleDockables = CreateList<IDockable>
                                (
                                    navigatorTool
                                ),
                                Alignment = Alignment.Left,
                                GripMode = GripMode.Visible
                            },
                            new ProportionalDockSplitter()
                            {
                                Id = "LeftPaneTopSplitter",
                                Title = "LeftPaneTopSplitter"
                            },
                            new ToolDock
                            {
                                Id = "LeftPaneBottom",
                                Title = "LeftPaneBottom",
                                Proportion = double.NaN,
                                ActiveDockable = navigatorTool,
                                VisibleDockables = CreateList<IDockable>
                                (
                                    navigatorTool
                                ),
                                Alignment = Alignment.Left,
                                GripMode = GripMode.Visible
                            }
                        )
                    },
                    new ProportionalDockSplitter()
                    {
                        Id = "LeftSplitter",
                        Title = "LeftSplitter"
                    },
                    MainDocumentDock
                )
            };

            var mainView = new MainViewModel
            {
                Id = "Main",
                Title = "Main",
                ActiveDockable = mainLayout,
                VisibleDockables = CreateList<IDockable>(mainLayout)
            };

            var root = CreateRootDock();

            root.Id = "Root";
            root.Title = "Root";
            root.ActiveDockable = mainView;
            root.DefaultDockable = mainView;
            root.VisibleDockables = CreateList<IDockable>(mainView);

            return root;
        }

        public void LoadProject(ProjectViewModel project)
        {
            AddDockable(MainDocumentDock, project);
            SetActiveDockable(project);
            SetFocusedDockable(MainDocumentDock, project);
            MainContext.Project = project;
        }

        public override void InitLayout(IDockable layout)
        {
            if (MainDocumentDock == null)
                throw new InvalidOperationException($"Состав главной панели не сформирован. Сперва требуется вызвать метод {nameof(CreateLayout)}");

            base.ContextLocator = new Dictionary<string, Func<object>>
            {
                [nameof(IRootDock)] = () => MainContext,
                [nameof(IProportionalDock)] = () => MainContext,
                [nameof(IDocumentDock)] = () => MainContext,
                [nameof(IToolDock)] = () => MainContext,
                [nameof(IProportionalDockSplitter)] = () => MainContext,
                [nameof(IDockWindow)] = () => MainContext,
                [nameof(IDocument)] = () => MainContext,
                [nameof(ITool)] = () => MainContext,
                ["Toolbox"] = () => MainContext,
                ["LeftPane"] = () => MainContext,
                ["LeftPaneTop"] = () => MainContext,
                ["LeftPaneTopSplitter"] = () => MainContext,
                ["LeftPaneBottom"] = () => MainContext,
                ["RightPane"] = () => MainContext,
                ["RightPaneTop"] = () => MainContext,
                ["RightPaneTopSplitter"] = () => MainContext,
                ["RightPaneBottom"] = () => MainContext,
                ["DocumentsPane"] = () => MainContext,
                ["LeftSplitter"] = () => MainContext,
                ["RightSplitter"] = () => MainContext,
                ["MainLayout"] = () => MainContext,
                ["Main"] = () => MainContext,
            };

            ActiveDockableChanged += MainDockFactory_ActiveDockableChanged;

            HostWindowLocator = new Dictionary<string, Func<IHostWindow>>
            {
                [nameof(IDockWindow)] = () =>
                {
                    var hostWindow = new HostWindow()
                    {
                        [binding: !Window.TitleProperty] = new Binding("ActiveDockable.Title")
                    };
                    return hostWindow;
                }
            };

            DockableLocator = new Dictionary<string, Func<IDockable?>>
            {

            };

            base.InitLayout(layout);

            SetActiveDockable(MainDocumentDock!);
            SetFocusedDockable(MainDocumentDock!, MainDocumentDock!.VisibleDockables?.FirstOrDefault());
        }

        private void MainDockFactory_ActiveDockableChanged(object? sender, Dock.Model.Core.Events.ActiveDockableChangedEventArgs e)
        {
            Debug.WriteLine($"Active changed {e.Dockable}");
            if (MainContext != null)
            {
                if (e.Dockable != null && e.Dockable is ProjectViewModel project)
                    MainContext.Project = project;
                else
                    MainContext.Project = null;
            }
        }
    }
}
