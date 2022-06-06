// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Messaging;
using Microsoft.Toolkit.Mvvm.Messaging.Messages;

namespace MvvmSample.Core.ViewModels.Widgets
{
    /// <summary>
    /// A viewmodel for a post widget.
    /// </summary>
    public sealed class ModelWidgetViewModel<TModel> : ObservableRecipient, IRecipient<PropertyChangedMessage<TModel>> where TModel : class
    {
        private TModel? model;

        /// <summary>
        /// Gets the currently selected post, if any.
        /// </summary>
        public TModel? Model
        {
            get => model;
            private set => SetProperty(ref model, value);
        }

        /// <inheritdoc/>
        public void Receive(PropertyChangedMessage<TModel> message)
        {
            if (message.Sender.GetType() == typeof(TModel))
            {
                this.Model = message.NewValue;
            }
        }
    }
}
