using System.ComponentModel;
using System.Runtime.CompilerServices;
using DraughtsGame.DataModels;

namespace DraughtsGame.UI.ViewModels;

public class MainWindowViewModel : INotifyPropertyChanged
{
    private Game game;

    public Game Game
    {
        get => game;
        set
        {
            game = value;
            OnPropertyChanged();
        }
    }
    
    public MainWindowViewModel()
    {
        Game = new Game();
    }

    // INotifyPropertyChanged implementation.
    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName]string propertyName = null) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}