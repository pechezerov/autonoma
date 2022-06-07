using Autonoma.UI.Presentation.Abstractions;
using Autonoma.UI.Presentation.Model;
using Newtonsoft.Json;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;

namespace Autonoma.UI.Presentation.ViewModels
{
    [JsonObject]
    public class ConnectorViewModel : ReactiveObject, IConnector
    {
        public string? Name { get; set; }

        [Reactive]
        public IPin? Start { get; set; }

        [Reactive]
        public IPin? End { get; set; }

        [Reactive]
        public ConnectorOrientation Orientation { get; set; }

        public IFrame? Parent { get; set; }

        [Reactive]
        public ConnectorType Type { get; set; }

        [Reactive]
        public double Offset { get; set; }

        public ConnectorViewModel() => ObservePins();

        public virtual bool CanRemove()
        {
            return true;
        }

        public virtual bool CanSelect()
        {
            return true;
        }

        private void ObservePins()
        {
            this.WhenAnyValue(x => x.Start)
                .Subscribe(start =>
                {
                    if (start?.Parent is not null)
                    {
                        start.Parent.WhenAnyValue(x => x.X).Subscribe(_ => this.RaisePropertyChanged(nameof(Start)));
                        start.Parent.WhenAnyValue(x => x.Y).Subscribe(_ => this.RaisePropertyChanged(nameof(Start)));
                    }
                    else
                    {
                        if (start is not null)
                        {
                            start.WhenAnyValue(x => x.X).Subscribe(_ => this.RaisePropertyChanged(nameof(Start)));
                            start.WhenAnyValue(x => x.Y).Subscribe(_ => this.RaisePropertyChanged(nameof(Start)));
                        }
                    }

                    if (start is not null)
                    {
                        start.WhenAnyValue(x => x.Alignment).Subscribe(_ => this.RaisePropertyChanged(nameof(Start)));
                    }
                });

            this.WhenAnyValue(x => x.End)
                .Subscribe(end =>
                {
                    if (end?.Parent is not null)
                    {
                        end.Parent.WhenAnyValue(x => x.X).Subscribe(_ => this.RaisePropertyChanged(nameof(End)));
                        end.Parent.WhenAnyValue(x => x.Y).Subscribe(_ => this.RaisePropertyChanged(nameof(End)));
                    }
                    else
                    {
                        if (end is not null)
                        {
                            end.WhenAnyValue(x => x.X).Subscribe(_ => this.RaisePropertyChanged(nameof(End)));
                            end.WhenAnyValue(x => x.Y).Subscribe(_ => this.RaisePropertyChanged(nameof(End)));
                        }
                    }

                    if (end is not null)
                    {
                        end.WhenAnyValue(x => x.Alignment).Subscribe(_ => this.RaisePropertyChanged(nameof(End)));
                    }
                });
        }
    }
}