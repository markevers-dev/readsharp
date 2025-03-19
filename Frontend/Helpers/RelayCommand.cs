using System.Windows.Input;

namespace Frontend.Helpers
{
    public class RelayCommand(Action<object> execute, Predicate<object> canExecute) : ICommand
    {
        private readonly Action<object> _execute = execute ?? throw new ArgumentNullException(nameof(execute));
        private readonly Predicate<object> _canExecute = canExecute;

        public RelayCommand(Action execute, Predicate<object> canExecute) : this(o => execute(), canExecute) { }

        public RelayCommand(Action<object> execute) : this(execute, o => true) { }

        public RelayCommand(Action execute) : this(o => execute()) { }

        public bool CanExecute(object parameter) =>
            _canExecute == null || _canExecute(parameter);

        public void Execute(object parameter) =>
            _execute(parameter);

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
    }
}

