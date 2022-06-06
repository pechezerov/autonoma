using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Autonoma.Core.MVVM.TaskCompletions
{
    public class NotifyValueTaskCompletion : TaskCompletion
    {
        public NotifyValueTaskCompletion(ValueTask task)
        {
            Task = task;
            if (!task.IsCompleted)
            {
                var _ = WatchTaskAsync(task);
            }
        }

        private async ValueTask WatchTaskAsync(ValueTask task)
        {
            try
            {
                await task;
            }
            catch (Exception ex)
            {
                OnExceptionThrown?.Invoke(ex);
            }

            if (IsNotHasNotificationSubscribers) return;

            Notify();
        }

        private void Notify()
        {
            List<string> updatedProperties = new()
            {
                nameof(IsCompleted),
                nameof(IsNotCompleted)
            };

            if (IsCanceled)
            {
                updatedProperties.Add(nameof(IsCanceled));
            }
            else if (IsFaulted)
            {
                updatedProperties.Add(nameof(IsFaulted));
            }
            else
            {
                updatedProperties.Add(nameof(IsSuccessfullyCompleted));
            }

            NotifyAboutPropertyChange(updatedProperties);
        }

        public ValueTask Task { get; }

        public event Action<Exception>? OnExceptionThrown;
        public override bool IsCompleted => Task.IsCompleted;
        public override bool IsNotCompleted => !Task.IsCompleted;
        public override bool IsSuccessfullyCompleted => Task.IsCompletedSuccessfully;
        public override bool IsCanceled => Task.IsCanceled;
        public override bool IsFaulted => Task.IsFaulted;
    }

    public class NotifyValueTaskCompletion<TResult> : TaskCompletion
    {
        public NotifyValueTaskCompletion(ValueTask<TResult> task)
        {
            Task = task;
            if (!task.IsCompleted)
            {
                var _ = WatchTaskAsync(task);
            }
        }

        private async ValueTask WatchTaskAsync(ValueTask<TResult> task)
        {
            try
            {
                await task;
            }
            catch (Exception ex)
            {
                OnExceptionThrown?.Invoke(ex);
            }

            if (IsNotHasNotificationSubscribers) return;

            Notify();
        }

        private void Notify()
        {
            List<string> updatedProperties = new()
            {
                nameof(IsCompleted),
                nameof(IsNotCompleted)
            };

            if (IsCanceled)
            {
                updatedProperties.Add(nameof(IsCanceled));
            }
            else if (IsFaulted)
            {
                updatedProperties.Add(nameof(IsFaulted));
            }
            else
            {
                updatedProperties.AddRange(new[]
                {
                nameof(IsSuccessfullyCompleted),
                nameof(Result)
            });
            }

            NotifyAboutPropertyChange(updatedProperties);
        }

        public ValueTask<TResult> Task { get; }
        public TResult? Result => IsSuccessfullyCompleted ? Task.Result : default;

        public event Action<Exception>? OnExceptionThrown;
        public override bool IsCompleted => Task.IsCompleted;
        public override bool IsNotCompleted => !Task.IsCompleted;
        public override bool IsSuccessfullyCompleted => Task.IsCompletedSuccessfully;
        public override bool IsCanceled => Task.IsCanceled;
        public override bool IsFaulted => Task.IsFaulted;
    }
}