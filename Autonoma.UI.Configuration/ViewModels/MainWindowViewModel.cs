using Autonoma.UI.Configuration.Abstractions;
using Autonoma.UI.Presentation.Model;
using Autonoma.UI.Presentation.ViewModels;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Dock.Model.Controls;
using Dock.Model.ReactiveUI.Controls;
using Microsoft.Extensions.DependencyInjection;
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
        private IProject? _project;

        #region Commands

        public ICommand? NewCommand { get; }

        public ICommand OpenCommand { get; }

        public ICommand SaveCommand { get; }

        public ICommand SaveAsCommand { get; }

        #endregion

        public IProject? Project
        {
            get => _project;
            set => this.RaiseAndSetIfChanged(ref _project, value);
        }

        [Reactive]
        public IRootDock Layout { get; set; }

        [Reactive]
        public IList<Tool> Tools { get; set; }

        public ICommand? ShowConnectionManagerCommand { get; }

        public IServiceProvider Provider { get; }

        public Interaction<ConnectionManagerViewModel, DataPointConnection?> ShowConnectionManagerDialog { get; }

        public MainWindowViewModel(IMainDockFactory dockFactory, IServiceProvider provider)
        {
            Provider = provider;
            Tools = new ObservableCollection<Tool>();
            dockFactory.MainContext = this;
            Layout = dockFactory.CreateLayout() ?? throw new InvalidOperationException("Не удалось инициализировать панель управления");
            dockFactory.InitLayout(Layout);

            // команды

            NewCommand = dockFactory.MainDocumentDock?.CreateDocument;

            OpenCommand = ReactiveCommand.CreateFromTask(async () =>
            {
                var taskPath = new OpenFileDialog().ShowAsync((Avalonia.Application.Current!.ApplicationLifetime! as IClassicDesktopStyleApplicationLifetime)!.MainWindow);
                string[]? paths = await taskPath;

                if (paths != null)
                {
                    var path = paths.First();

                    File.Copy(path, "configuration.db3", true);

                    var serializer = Provider.GetRequiredService<IProjectSerializer>();
                    var project = serializer.DeserializeProject<IProject>(path);
                    if (project is ProjectViewModel projectVm)
                        dockFactory.LoadProject(projectVm);
                }
            });

            SaveCommand = ReactiveCommand.CreateFromTask(async () =>
            {
                if (Project == null)
                    return;
                var path = Project.FilePath;
                if (path != null)
                {
                    var serializer = provider.GetRequiredService<IProjectSerializer>();
                    var projectDate = serializer.SerializeProject(Project);
                    File.WriteAllText(path, projectDate);
                }
            });

            SaveAsCommand = ReactiveCommand.CreateFromTask(async () =>
            {
                if (Project == null)
                    return;
                var taskPath = new SaveFileDialog().ShowAsync((Avalonia.Application.Current!.ApplicationLifetime! as IClassicDesktopStyleApplicationLifetime)!.MainWindow);
                var path = await taskPath;
                if (path != null)
                {
                    var serializer = provider.GetRequiredService<IProjectSerializer>(); 
                    var projectDate = serializer.SerializeProject(Project);
                    File.WriteAllText(path, projectDate);
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