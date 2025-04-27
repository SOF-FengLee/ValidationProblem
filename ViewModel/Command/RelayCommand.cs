using System.Windows.Input;

namespace TestValidation.ViewModel.Command
{
    public class RelayCommand<T> : ICommand
    {
        private readonly Action<T> _execute;
        private readonly Predicate<T>? _canExecute = null;

        public RelayCommand(Action<T> execute) : this(execute, null) { }

        public RelayCommand(Action<T>? execute, Predicate<T>? predicate)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = predicate;
        }

        public event EventHandler? CanExecuteChanged
        {
            add
            {
                CommandManager.RequerySuggested += value;
            }

            remove
            {
                CommandManager.RequerySuggested -= value;
            }
        }

        public bool CanExecute(object? parameter)
        {
            if (_canExecute is null)
                return true;

            if (parameter is null && (typeof(T).IsClass || Nullable.GetUnderlyingType(typeof(T)) is not null))
                return _canExecute(default!);

            return parameter is T converted && _canExecute(converted);
        }

        public void Execute(object? parameter)
        {
            _execute((T)parameter);
        }
    }
}
