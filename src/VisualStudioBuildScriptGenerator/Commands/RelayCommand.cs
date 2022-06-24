using System;
using System.Windows.Input;

namespace VisualStudioBuildScriptGenerator
{
    class RelayCommand : ICommand
    {
        Action<object> _execute;
        Func<object, bool> _canExecute;

        public RelayCommand(Action<object> execute) : this(execute, (i) => true)
        {
        }
        public RelayCommand(Action<object> execute, Func<object, bool> canExecute)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter) => _canExecute(parameter);

        public void Execute(object parameter) => _execute(parameter);
    }
}
