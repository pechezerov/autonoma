using Autonoma.UI.Configuration.Abstractions;
using Autonoma.UI.FrameEditor.ViewModels;
using Autonoma.UI.Presentation.Model;
using Autonoma.UI.Presentation.ViewModels;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Dock.Model.Controls;
using Dock.Model.ReactiveUI.Controls;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reactive.Linq;
using System.Windows.Input;

namespace Autonoma.UI.Configuration.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        protected IRootDock _layout;
        protected IProjectSerializer _serializer;

        #region

        public ICommand? NewCommand { get; }

        public ICommand OpenCommand { get; }

        public ICommand SaveAsCommand { get; }

        #endregion

        [Reactive]
        public ProjectViewModel? Project { get; set; }

        public IRootDock Layout
        {
            get => _layout;
            set => this.RaiseAndSetIfChanged(ref _layout, value);
        }
        public IList<Tool> Tools { get; set; }

        public ICommand? ShowConnectionManagerCommand { get; }

        public Interaction<ConnectionManagerViewModel, DataPointConnection?> ShowConnectionManagerDialog { get; }

        public MainWindowViewModel(IMainDockFactory dockFactory, IProjectSerializer serializer)
        {
            _serializer = serializer;
            Tools = new ObservableCollection<Tool>();
            dockFactory.MainContext = this;
            _layout = dockFactory.CreateLayout() ?? throw new InvalidOperationException("Не удалось инициализировать панель управления");
            dockFactory.InitLayout(_layout);

            // команды

            NewCommand = dockFactory.MainDocumentDock?.CreateDocument;

            OpenCommand = ReactiveCommand.CreateFromTask(async () =>
            {
                var taskPath = new OpenFileDialog().ShowAsync((Avalonia.Application.Current!.ApplicationLifetime! as IClassicDesktopStyleApplicationLifetime)!.MainWindow);
                string[]? paths = await taskPath;

                if (paths != null)
                {
                    var path = paths.First();
                    var projectData = File.ReadAllText(path);
                    var project = _serializer.DeserializeProject(projectData);
                    if (project != null)
                    {
                        project.FilePath = path;
                        dockFactory.LoadProject(project);
                    }
                }
            });

            SaveAsCommand = ReactiveCommand.CreateFromTask(async () =>
            {
                if (Project == null)
                    return;
                var taskPath = new SaveFileDialog().ShowAsync((Avalonia.Application.Current!.ApplicationLifetime! as IClassicDesktopStyleApplicationLifetime)!.MainWindow);
                var frameData = _serializer.SerializeProject(Project);
                var path = await taskPath;
                if (path != null)
                    File.WriteAllText(path, frameData);
            });

            SaveAsCommand = ReactiveCommand.CreateFromTask(async () =>
            {
                if (Project == null)
                    return;
                var taskPath = new SaveFileDialog().ShowAsync((Avalonia.Application.Current!.ApplicationLifetime! as IClassicDesktopStyleApplicationLifetime)!.MainWindow);
                var path = await taskPath;
                if (path != null)
                {
                    var frameData = _serializer.SerializeProject(Project);
                    File.WriteAllText(path, frameData);
                }
            });

            ShowConnectionManagerCommand = ReactiveCommand.CreateFromTask(async () =>
            {
                var store = new ConnectionManagerViewModel();
                var result = await ShowConnectionManagerDialog?.Handle(store);
            });

            // диалоги

            ShowConnectionManagerDialog = new Interaction<ConnectionManagerViewModel, DataPointConnection?>();

            // события

            this.WhenAnyValue(x => x.Project)
                .Subscribe(doc =>
                {
                   
                });
        }
    }
}