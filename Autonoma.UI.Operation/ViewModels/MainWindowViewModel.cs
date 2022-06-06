using Autonoma.Domain;
using Autonoma.UI.Presentation.Design;
using Autonoma.UI.Presentation.ViewModels;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Autonoma.UI.Operation.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private readonly CommunicationService _communicationService;

        public ObservableCollection<FrameViewModel> Frames { get; set; } = new ObservableCollection<FrameViewModel>();

        [Reactive]
        public FrameViewModel? SelectedFrame { get; set; }

        public MainWindowViewModel(CommunicationService communicationService)
        {
            _communicationService = communicationService;
            _communicationService.OnUpdatesReceived = UpdatesReceived;

            this.WhenAnyValue(x => x.SelectedFrame)
                .Subscribe(Subscribe!);
        }

        private void Subscribe(FrameViewModel? selectedFrame)
        {
            _communicationService
                .Subscribe((SelectedFrame?.Nodes
                    .Where(mf => mf.LinkedDataPointId != null)
                    .Select(mf => mf.LinkedDataPointId)
                    .Cast<int>() 
                        ?? Enumerable.Empty<int>())
                    .Distinct()
                    .ToList());
        }

        private void UpdatesReceived(object? sender, List<DataPointInfo> updates)
        {
            // поступившие данные могут быть направлены:
            // - на кадр
            // - в модуль обобщенной сигнализации

            if (SelectedFrame != null)
                SelectedFrame.Update(updates);
        }
    }
}
