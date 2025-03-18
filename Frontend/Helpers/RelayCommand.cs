using System.Windows.Input;

namespace Frontend.Helpers
{
    public class RelayCommand : ICommand
    {
        private readonly Action<object> _execute;
        private readonly Predicate<object> _canExecute;

        // Constructor for commands that accept an object parameter.
        public RelayCommand(Action<object> execute, Predicate<object> canExecute)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        // Overload for commands that do not require a parameter.
        public RelayCommand(Action execute, Predicate<object> canExecute) : this(o => execute(), canExecute) { }

        // Overload for commands that do not require a predicate.
        public RelayCommand(Action<object> execute) : this(execute, o => true) { }

        // Overload for commands that do not require a predicate.
        public RelayCommand(Action execute) : this(o => execute()) { }

        public bool CanExecute(object parameter) =>
            _canExecute == null ? true : _canExecute(parameter);

        public void Execute(object parameter) =>
            _execute(parameter);

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
    }
}

