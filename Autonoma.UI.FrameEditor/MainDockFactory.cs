using Autonoma.UI.FrameEditor.Models.Tools;
using Autonoma.UI.FrameEditor.ViewModels;
using Autonoma.UI.FrameEditor.ViewModels.Documents;
using Autonoma.UI.FrameEditor.ViewModels.Tools;
using Autonoma.UI.Presentation.Design;
using Autonoma.UI.Presentation.ViewModels;
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

namespace Autonoma.UI.FrameEditor
{
    public class MainDockFactory : Factory, IMainDockFactory
    {
        public IDocumentDock MainDocumentDock { get; set; } = null!;

        public MainWindowViewModel MainContext { get; set; } = null!;

        public MainDockFactory() { }

        public override IRootDock CreateLayout()
        {
            var toolboxTool = new ToolboxToolViewModel
            {
                Id = "Toolbox",
                Title = "Toolbox",
                Context = MainContext
            };
            MainContext.Tools.Add(toolboxTool);

            var frameMonitorTool = new FrameMonitorToolViewModel
            {
                Id = "FrameMonitor",
                Title = "FrameMonitor",
                Context = MainContext
            };
            MainContext.Tools.Add(frameMonitorTool);

            var propertiesTool = new PropertiesToolViewModel
            {
                Id = "Properties",
                Title = "Properties",
                Context = MainContext
            };
            MainContext.Tools.Add(propertiesTool);

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
                var frame = DesignFactory.CreateFrame("Новый кадр");
                var document = new DocumentViewModel(frame);
                LoadDocument(document);
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
                                ActiveDockable = toolboxTool,
                                VisibleDockables = CreateList<IDockable>
                                (
                                    toolboxTool,
                                    frameMonitorTool
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
                                ActiveDockable = propertiesTool,
                                VisibleDockables = CreateList<IDockable>
                                (
                                    propertiesTool
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

        public void LoadDocument(DocumentViewModel document)
        {
            AddDockable(MainDocumentDock, document);
            SetActiveDockable(document);
            SetFocusedDockable(MainDocumentDock, document);
            MainContext.Documents.Add(document);
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
                ["FrameMonitor"] = () => MainContext,
                ["Properties"] = () => MainContext,
                ["Connections"] = () => MainContext,
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
                if (e.Dockable != null && e.Dockable is DocumentViewModel docViewModel)
                    MainContext.CurrentDocument = docViewModel;
                else
                    MainContext.CurrentDocument = null;
            }
        }
    }
}
