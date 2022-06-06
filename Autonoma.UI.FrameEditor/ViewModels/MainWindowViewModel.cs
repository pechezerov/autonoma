using Autonoma.UI.FrameEditor.Services.ServiceBus.Messages;
using Autonoma.UI.FrameEditor.ViewModels.Documents;
using Autonoma.UI.FrameEditor.ViewModels.Tools;
using Autonoma.UI.FrameEditor.Views.Tools;
using Autonoma.UI.Presentation.Abstractions;
using Autonoma.UI.Presentation.Model;
using Autonoma.UI.Presentation.Services;
using Autonoma.UI.Presentation.ViewModels;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Dock.Model.Controls;
using Dock.Model.ReactiveUI.Controls;
using DynamicData;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Reactive.Linq;
using System.Windows.Input;
using IElementFactory = Autonoma.UI.Presentation.Abstractions.IElementFactory;

namespace Autonoma.UI.FrameEditor.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        protected DocumentViewModel? _currentDocument;
        protected IElementFactory _elementFactory;
        protected bool _isEditMode;
        protected IRootDock _layout;
        protected IFrameSerializer _serializer;

        protected IList<IElementPrototype> _prototypes;
        protected IList<Tool> _tools;
        protected IList<DocumentViewModel> _documents;

        #region

        public ICommand? NewCommand { get; }

        public ICommand OpenCommand { get; }

        public ICommand SaveAsCommand { get; }

        #endregion

        public DocumentViewModel? CurrentDocument
        {
            get => _currentDocument;
            set
            {
                this.RaiseAndSetIfChanged(ref _currentDocument, value);
                MessageBus.Current.SendMessage(new SelectedDocumentChangedMessage(_currentDocument));
            }
        }

        public IElementFactory ElementFactory
        {
            get => _elementFactory;
            set => this.RaiseAndSetIfChanged(ref _elementFactory, value);
        }

        public bool IsEditMode
        {
            get => _isEditMode;
            set => this.RaiseAndSetIfChanged(ref _isEditMode, value);
        }

        public IRootDock Layout
        {
            get => _layout;
            set => this.RaiseAndSetIfChanged(ref _layout, value);
        }

        public IFrameSerializer Serializer
        {
            get => _serializer;
            set => this.RaiseAndSetIfChanged(ref _serializer, value);
        }

        public ICommand? ShowConnectionManagerCommand { get; }

        public Interaction<ConnectionManagerViewModel, DataPointConnection?> ShowConnectionManagerDialog { get; }

        public IList<IElementPrototype> Prototypes
        {
            get => _prototypes;
            set => this.RaiseAndSetIfChanged(ref _prototypes, value);
        }

        public IList<Tool> Tools
        {
            get => _tools;
            set => this.RaiseAndSetIfChanged(ref _tools, value);
        }
        public IList<DocumentViewModel> Documents
        {
            get => _documents;
            set => this.RaiseAndSetIfChanged(ref _documents, value);
        }

        public MainWindowViewModel() : this (new MainDockFactory(), new JsonFrameSerializer(), new Presentation.Infrastructure.ElementFactory())
        {

        }

        public MainWindowViewModel(IMainDockFactory dockFactory, IFrameSerializer serializer, IElementFactory elementFactory)
        {
            _serializer = serializer;
            _elementFactory = elementFactory;

            _prototypes = elementFactory.CreateToolbox();
            _tools = new ObservableCollection<Tool>();
            _documents = new ObservableCollection<DocumentViewModel>();

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
                    foreach (var path in paths)
                    {
                        var frameData = File.ReadAllText(path);
                        var frame = _serializer.DeserializeFrame(frameData);
                        if (frame != null)
                        {
                            var document = new DocumentViewModel(frame);
                            document.FilePath = path;
                            dockFactory.LoadDocument(document);
                        }
                    }
                }
            });

            SaveAsCommand = ReactiveCommand.CreateFromTask(async () =>
            {
                if (CurrentDocument == null)
                    return;
                var taskPath = new SaveFileDialog().ShowAsync((Avalonia.Application.Current!.ApplicationLifetime! as IClassicDesktopStyleApplicationLifetime)!.MainWindow);
                var frameData = serializer.Serialize(CurrentDocument);
                var path = await taskPath;
                if (path != null)
                    File.WriteAllText(path, frameData);
            });

            SaveAsCommand = ReactiveCommand.CreateFromTask(async () =>
            {
                if (CurrentDocument == null)
                    return;
                var taskPath = new SaveFileDialog().ShowAsync((Avalonia.Application.Current!.ApplicationLifetime! as IClassicDesktopStyleApplicationLifetime)!.MainWindow);
                var path = await taskPath;
                if (path != null)
                {
                    var frameData = serializer.Serialize(CurrentDocument.Frame);
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

            this.WhenAnyValue(x => x.CurrentDocument)
                .Subscribe(doc => 
                {
                    foreach (var tool in Tools)
                    {
                        if (tool is PropertiesToolViewModel propTool)
                            propTool.EditedObject = doc?.Frame?.SelectedNode ?? doc?.Frame;
                    }
                });
            this.WhenAnyValue(x => x.CurrentDocument.Frame.SelectedNode)
                .Subscribe(node =>
                {
                    foreach (var tool in Tools)
                    {
                        if (tool is PropertiesToolViewModel propTool)
                            propTool.EditedObject = node ?? CurrentDocument?.Frame;
                    }
                });
        }
    }
}