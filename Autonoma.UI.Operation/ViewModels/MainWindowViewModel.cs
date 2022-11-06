using Autonoma.Domain;
using Autonoma.Primitives;
using Autonoma.UI.Presentation.ViewModels;
using Core2D.ViewModels.Containers;
using Core2D.ViewModels.Editor;
using Microsoft.Extensions.DependencyInjection;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace Autonoma.UI.Operation.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private readonly CommunicationService _communicationService;
        private ILookup<int, IDataPointConsumer>? _subscriptions;

        public ProjectContainerViewModel Project { get; set; }

        [Reactive]
        public PageContainerViewModel? SelectedFrame { get; set; }

        [Reactive]
        public List<PageContainerViewModel> Frames { get; set; }

        public MainWindowViewModel(ProjectContainerViewModel project, CommunicationService communicationService)
        {
            _communicationService = communicationService;
            _communicationService.OnUpdatesReceived = UpdatesReceived;

            Frames = project.Documents.SelectMany(d => d.Pages).ToList();

            this.WhenAnyValue(x => x.SelectedFrame)
                .Subscribe(Subscribe!);
        }

        private void Subscribe(PageContainerViewModel? selectedFrame)
        {
            if (SelectedFrame != null)
            {
                _subscriptions = SelectedFrame.Layers.SelectMany(l => l.Shapes)
                    .Where(sh => sh is IDataPointConsumer)
                    .Select(sh => sh.Properties.FirstOrDefault(p => p.Name == "Tag"))
                    .ToLookup(p => p == null ? -1 : (p.Value == null ? -1 : Int32.Parse(p.Value)), p => p as IDataPointConsumer);
            }
            else
            {
                _subscriptions = null;
            }

            _communicationService
                .Subscribe((_subscriptions?
                    .Select(s => s.Key) ?? Enumerable.Empty<int>())
                    .ToList());
        }

        private void UpdatesReceived(object? sender, List<DataPointInfo> updates)
        {
            // поступившие данные могут быть направлены:
            // - на кадр
            // - в модуль обобщенной сигнализации

            foreach (var update in updates)
            {
                if (_subscriptions.Contains(update.DataPointId))
                {
                    var subscriptionSet = _subscriptions[update.DataPointId];
                    foreach (var subscriptionSetItem in subscriptionSet)
                    {
                        subscriptionSetItem.Update(update);
                    }
                }
            }
        }
    }
}
