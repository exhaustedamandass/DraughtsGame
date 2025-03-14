using System;
using System.Windows.Input;

namespace DraughtsGame.UI;

public class RelayCommand : ICommand
{
    private readonly Action<object> _execute;
    private readonly Func<object, bool> _canExecute;

    public RelayCommand(Action<object> execute, Func<object, bool> canExecute = null)
    {
        _execute = execute ?? throw new ArgumentNullException(nameof(execute));
        _canExecute = canExecute;
    }

    public bool CanExecute(object parameter) => _canExecute?.Invoke(parameter) ?? true;

    public void Execute(object parameter) => _execute(parameter);

    // This event is part of the ICommand interface.  
    // Raise it if something changes that affects whether the command can execute.
    public event EventHandler CanExecuteChanged;
}