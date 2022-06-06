using Autonoma.Domain;
using Autonoma.UI.Presentation.Abstractions;
using Autonoma.UI.Presentation.Attributes;
using Autonoma.UI.Presentation.Controls;
using Autonoma.UI.Presentation.Model;
using Newtonsoft.Json;
using ReactiveUI.Fody.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Autonoma.UI.Presentation.ViewModels
{
    [JsonObject]
    public class ElementViewModel : ViewModelBase, IElement
    {
        [Reactive]
        public string Name { get; set; } = "";

        [Browsable(false)]
        public IElement? Parent { get; set; }

        [Reactive]
        public double X { get; set; }

        [Reactive]
        public double Y { get; set; }

        [Reactive]
        public double Width { get; set; }

        [Reactive]
        public double Height { get; set; }

        [Browsable(false)]
        public IElementContent? Content { get; set; }

        [Reactive]
        [Browsable(false)]
        public ObservableCollection<IPin> Pins { get; set; } = new ObservableCollection<IPin>();

        [JsonIgnore]
        IList<IPin>? IElement.Pins => Pins;

        [Browsable(false)]
        [Reactive]
        public int? LinkedDataPointId { get; set; }

        object? IElement.Content
        {
            get => Content;
            set
            {
                if (value is IElementContent el)
                    Content = el;
                else
                    Content = null;
            }
        }

        public virtual bool CanSelect()
        {
            return true;
        }

        public virtual bool CanRemove()
        {
            return true;
        }

        public virtual bool CanMove()
        {
            return true;
        }

        public virtual bool CanResize()
        {
            return true;
        }

        public virtual void Move(double deltaX, double deltaY)
        {
            X += deltaX;
            Y += deltaY;
        }

        public virtual void Resize(double deltaX, double deltaY, NodeResizeDirection direction)
        {
            throw new NotImplementedException();
        }

        public void AddPin(PinAttribute pinMetaData)
        {
            AddPin(pinMetaData.X, pinMetaData.Y, pinMetaData.Width, pinMetaData.Height, pinMetaData.Alignment, pinMetaData.Direction, pinMetaData.Name);
        }

        public IPin AddPin(double x, double y, double width, double height, PinAlignment alignment = PinAlignment.None, PinDirection direction = PinDirection.None, string? name = null)
        {
            var pin = new PinViewModel
            {
                Name = name,
                Parent = this,
                X = x,
                Y = y,
                Width = width,
                Height = height,
                Alignment = alignment,
                Direction = direction,
            };

            Pins.Add(pin);

            return pin;
        }

        public void Update(List<DataPointInfo> updates)
        {
            foreach (var update in updates)
                Content?.Update(update);
        }
    }
}
