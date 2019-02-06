// async command doesn't capture exceptions (anymore)
// https://github.com/StephenCleary/Mvvm.Async/issues/6#issuecomment-249306870

// ReSharper disable once CheckNamespace
namespace Nito.Mvvm
{
    using System;
    using System.ComponentModel;
    using System.Threading.Tasks;
    using System.Windows.Input;

    /// <summary>
    /// A basic asynchronous command, which (by default) is disabled while the command is executing.C:\Repositories\Mine\ImageHash\demo\Demo\External\Class1.cs
    /// </summary>
    public sealed class CapturingExceptionAsyncCommand : AsyncCommandBase, INotifyPropertyChanged
    {
        /// <summary>
        /// The implementation of <see cref="IAsyncCommand.ExecuteAsync(object)"/>.
        /// </summary>
        private readonly Func<object, Task> executeAsync;

        /// <summary>
        /// The implementation of <see cref="ICommand.CanExecute(object)"/>.
        /// </summary>
        private readonly Func<object, bool> canExecute;

        /// <summary>
        /// Creates a new asynchronous command, with the specified asynchronous delegate as its implementation.
        /// </summary>
        /// <param name="executeAsync">The implementation of <see cref="IAsyncCommand.ExecuteAsync(object)"/>.</param>
        /// <param name="canExecute">The implementation of <see cref="ICommand.CanExecute(object)"/>.</param>
        /// <param name="canExecuteChangedFactory">The factory for the implementation of <see cref="ICommand.CanExecuteChanged"/>.</param>
        public CapturingExceptionAsyncCommand(Func<object, Task> executeAsync, Func<object, bool> canExecute, Func<object, ICanExecuteChanged> canExecuteChangedFactory)
            : base(canExecuteChangedFactory)
        {
            this.executeAsync = executeAsync;
            this.canExecute = canExecute;
        }

        /// <summary>
        /// Creates a new asynchronous command, with the specified asynchronous delegate as its implementation.
        /// </summary>
        /// <param name="executeAsync">The implementation of <see cref="IAsyncCommand.ExecuteAsync(object)"/>.</param>
        public CapturingExceptionAsyncCommand(Func<object, Task> executeAsync)
            : this(executeAsync, _ => true, CanExecuteChangedFactories.DefaultCanExecuteChangedFactory)
        {
        }

        /// <summary>
        /// Creates a new asynchronous command, with the specified asynchronous delegate as its implementation.
        /// </summary>
        /// <param name="executeAsync">The implementation of <see cref="IAsyncCommand.ExecuteAsync(object)"/>.</param>
        public CapturingExceptionAsyncCommand(Func<Task> executeAsync)
            : this(_ => executeAsync(), _ => true, CanExecuteChangedFactories.DefaultCanExecuteChangedFactory)
        {
        }

        /// <summary>
        /// Creates a new asynchronous command, with the specified asynchronous delegate as its implementation.
        /// </summary>
        /// <param name="executeAsync">The implementation of <see cref="IAsyncCommand.ExecuteAsync(object)"/>.</param>
        /// <param name="canExecute">The implementation of <see cref="ICommand.CanExecute(object)"/>.</param>
        public CapturingExceptionAsyncCommand(Func<object, Task> executeAsync, Func<object, bool> canExecute)
            : this(executeAsync, canExecute, CanExecuteChangedFactories.DefaultCanExecuteChangedFactory)
        {
        }

        /// <summary>
        /// Creates a new asynchronous command, with the specified asynchronous delegate as its implementation.
        /// </summary>
        /// <param name="executeAsync">The implementation of <see cref="IAsyncCommand.ExecuteAsync(object)"/>.</param>
        /// <param name="canExecute">The implementation of <see cref="ICommand.CanExecute(object)"/>.</param>
        /// <param name="canExecuteChangedFactory">The factory for the implementation of <see cref="ICommand.CanExecuteChanged"/>.</param>
        public CapturingExceptionAsyncCommand(Func<Task> executeAsync, Func<bool> canExecute, Func<object, ICanExecuteChanged> canExecuteChangedFactory)
            : this(_ => executeAsync(), _ => canExecute(), canExecuteChangedFactory)
        {
        }

        /// <summary>
        /// Creates a new asynchronous command, with the specified asynchronous delegate as its implementation.
        /// </summary>
        /// <param name="executeAsync">The implementation of <see cref="IAsyncCommand.ExecuteAsync(object)"/>.</param>
        /// <param name="canExecute">The implementation of <see cref="ICommand.CanExecute(object)"/>.</param>
        public CapturingExceptionAsyncCommand(Func<Task> executeAsync, Func<bool> canExecute)
            : this(_ => executeAsync(), _ => canExecute(), CanExecuteChangedFactories.DefaultCanExecuteChangedFactory)
        {
        }

        /// <summary>
        /// Raised when any properties on this instance have changed.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Represents the most recent execution of the asynchronous command. Returns <c>null</c> until the first execution of this command.
        /// </summary>
        public NotifyTask Execution { get; private set; }

        /// <summary>
        /// Whether the asynchronous command is currently executing.
        /// </summary>
        /// <returns><c>true</c> when the command is currently executing. Otherwise <c>false</c>.</returns>
        public bool IsExecuting => Execution != null && Execution.IsNotCompleted;

        /// <summary>
        /// Executes the asynchronous command.
        /// </summary>
        /// <param name="parameter">The parameter for the command.</param>
        public override async Task ExecuteAsync(object parameter)
        {
            var tcs = new TaskCompletionSource<object>();
            Execution = NotifyTask.Create(DoExecuteAsync(tcs.Task, executeAsync, parameter));
            OnCanExecuteChanged();
            var propertyChanged = PropertyChanged;
            propertyChanged?.Invoke(this, PropertyChangedEventArgsCache.Instance.Get("Execution"));
            propertyChanged?.Invoke(this, PropertyChangedEventArgsCache.Instance.Get("IsExecuting"));
            tcs.SetResult(null);
            await Execution.TaskCompleted;
            OnCanExecuteChanged();
            PropertyChanged?.Invoke(this, PropertyChangedEventArgsCache.Instance.Get("IsExecuting"));

            // await Execution.Task;
        }

        /// <summary>
        /// Raises <see cref="ICommand.CanExecuteChanged"/>.
        /// </summary>
        public new void OnCanExecuteChanged() => base.OnCanExecuteChanged();

        /// <summary>
        /// The implementation of <see cref="ICommand.CanExecute(object)"/>. Returns <c>false</c> whenever the async command is in progress.
        /// </summary>
        /// <param name="parameter">The parameter for the command.</param>
        protected override bool CanExecute(object parameter) => !IsExecuting && canExecute(parameter);

        private static async Task DoExecuteAsync(Task precondition, Func<object, Task> executeAsync, object parameter)
        {
            await precondition;
            await executeAsync(parameter);
        }
    }
}
