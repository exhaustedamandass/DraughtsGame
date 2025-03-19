using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using DraughtsGame.UI.ViewModels;

namespace DraughtsGame.UI.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
#if DEBUG
        this.AttachDevTools();
#endif
    }
    
    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
    
    private void OnMoveCompleted(object sender, System.EventArgs e)
    {
        if (DataContext is MainWindowViewModel viewModel)
        {
            viewModel.OnPlayerMoveCompleted();
        }
    }
}