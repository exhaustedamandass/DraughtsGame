using System;
using System.Windows.Input;

namespace DraughtsGame.Commands;

public class RelayCommand : ICommand
{
    public event EventHandler? CanExecuteChanged;

    private Action<object> _Execute { get; set; }
    private Predicate<object> _CanExecute { get; set; }

    public RelayCommand(Action<object> execute, Predicate<object> canExecute)
    {
        _Execute = execute;
        _CanExecute = canExecute;
    }
    
    public bool CanExecute(object? parameter)
    {
        return parameter != null && _CanExecute(parameter);
    }

    public void Execute(object? parameter)
    {
        if (parameter != null) _Execute(parameter);
    }
}