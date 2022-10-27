using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;

namespace Autonoma.Core.MVVM.TaskCompletions
{
    public abstract class TaskCompletion : INotifyPropertyChanged
    {
        public static NotifyTaskCompletion Create(Task task) => new NotifyTaskCompletion(task);
        public static NotifyTaskCompletion<T> Create<T>(Task<T> task) => new NotifyTaskCompletion<T>(task);
        public static NotifyValueTaskCompletion Create(ValueTask task) => new NotifyValueTaskCompletion(task);
        public static NotifyValueTaskCompletion<T> Create<T>(ValueTask<T> task) => new NotifyValueTaskCompletion<T>(task);
        public static NotifyTaskCompletion Create(Func<Task> task) => new NotifyTaskCompletion(task.Invoke());
        public static NotifyTaskCompletion<T> Create<T>(Func<Task<T>> task) => new NotifyTaskCompletion<T>(task.Invoke());
        public static NotifyValueTaskCompletion Create(Func<ValueTask> task) => new NotifyValueTaskCompletion(task.Invoke());
        public static NotifyValueTaskCompletion<T> Create<T>(Func<ValueTask<T>> task) => new NotifyValueTaskCompletion<T>(task.Invoke());

        public static TaskCompletion CreateAsBase(Task task) => new NotifyTaskCompletion(task);
        public static TaskCompletion CreateAsBase<T>(Task<T> task) => new NotifyTaskCompletion<T>(task);
        public static TaskCompletion CreateAsBase(ValueTask task) => new NotifyValueTaskCompletion(task);
        public static TaskCompletion CreateAsBase<T>(ValueTask<T> task) => new NotifyValueTaskCompletion<T>(task);
        public static TaskCompletion CreateAsBase(Func<Task> task) => new NotifyTaskCompletion(task.Invoke());
        public static TaskCompletion CreateAsBase<T>(Func<Task<T>> task) => new NotifyTaskCompletion<T>(task.Invoke());
        public static TaskCompletion CreateAsBase(Func<ValueTask> task) => new NotifyValueTaskCompletion(task.Invoke());
        public static TaskCompletion CreateAsBase<T>(Func<ValueTask<T>> task) => new NotifyValueTaskCompletion<T>(task.Invoke());


        public event PropertyChangedEventHandler? PropertyChanged;

        public abstract bool IsCompleted { get; }
        public abstract bool IsNotCompleted { get; }
        public abstract bool IsSuccessfullyCompleted { get; }
        public abstract bool IsCanceled { get; }
        public abstract bool IsFaulted { get; }

        protected bool IsNotHasNotificationSubscribers => PropertyChanged is null;

        protected void NotifyAboutPropertyChange(IEnumerable<string> propertiesChangedNames)
        {
            if (PropertyChanged is not { } propertyChanged) return;
            foreach (var propertyName in propertiesChangedNames)
            {
                propertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}