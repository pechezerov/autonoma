using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Autonoma.Core.MVVM.TaskCompletions
{
    public sealed class NotifyTaskCompletion : TaskCompletion
    {
        public NotifyTaskCompletion(Task task)
        {
            Task = task;
            if (!task.IsCompleted)
            {
                var _ = WatchTaskAsync(task);
            }
        }

        private async Task WatchTaskAsync(Task task)
        {
            try
            {
                await task;
            }
            catch
            {
                // ignored
            }

            if (IsNotHasNotificationSubscribers) return;

            Notify();
        }

        private void Notify()
        {
            List<string> updatedProperties = new()
            {
                nameof(Status),
                nameof(IsCompleted),
                nameof(IsNotCompleted)
            };

            if (IsCanceled)
            {
                updatedProperties.Add(nameof(IsCanceled));
            }
            else if (IsFaulted)
            {
                updatedProperties.AddRange(new[]
                {
                    nameof(IsCanceled),
                    nameof(Exception),
                    nameof(InnerException),
                    nameof(ErrorMessage)
                });
            }
            else
            {
                updatedProperties.Add(nameof(IsSuccessfullyCompleted));
            }

            NotifyAboutPropertyChange(updatedProperties);
        }

        public Task Task { get; }

        public TaskStatus Status => Task.Status;

        public AggregateException? Exception => Task.Exception;
        public Exception? InnerException => Exception?.InnerException;
        public string? ErrorMessage => InnerException?.Message;

        public override bool IsCompleted => Task.IsCompleted;
        public override bool IsNotCompleted => !Task.IsCompleted;
        public override bool IsSuccessfullyCompleted => Task.Status == TaskStatus.RanToCompletion;
        public override bool IsCanceled => Task.IsCanceled;
        public override bool IsFaulted => Task.IsFaulted;
    }

    public class NotifyTaskCompletion<TResult> : TaskCompletion
    {
        public NotifyTaskCompletion(Task<TResult> task)
        {
            Task = task;
            if (!task.IsCompleted)
            {
                var _ = WatchTaskAsync(task);
            }
        }

        private async Task WatchTaskAsync(Task task)
        {
            try
            {
                await task;
            }
            catch
            {
                // ignored
            }

            if (IsNotHasNotificationSubscribers) return;

            Notify();
        }

        private void Notify()
        {
            List<string> updatedProperties = new()
            {
                nameof(Status),
                nameof(IsCompleted),
                nameof(IsNotCompleted)
            };

            if (IsCanceled)
            {
                updatedProperties.Add(nameof(IsCanceled));
            }
            else if (IsFaulted)
            {
                updatedProperties.AddRange(new[]
                {
                    nameof(IsFaulted),
                    nameof(Exception),
                    nameof(InnerException),
                    nameof(ErrorMessage)
                });
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

        public Task<TResult> Task { get; }
        public TResult? Result => Task.Status == TaskStatus.RanToCompletion ? Task.Result : default;
        public TaskStatus Status => Task.Status;

        public AggregateException? Exception => Task.Exception;
        public Exception? InnerException => Exception?.InnerException;
        public string? ErrorMessage => InnerException?.Message;

        public override bool IsCompleted => Task.IsCompleted;
        public override bool IsNotCompleted => !Task.IsCompleted;
        public override bool IsSuccessfullyCompleted => Task.Status == TaskStatus.RanToCompletion;
        public override bool IsCanceled => Task.IsCanceled;
        public override bool IsFaulted => Task.IsFaulted;
    }
}