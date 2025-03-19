using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using DraughtsGame.Commands;
using DraughtsGame.DataModels;

namespace DraughtsGame.UI.ViewModels;

public class MainWindowViewModel : INotifyPropertyChanged
{
    private Game game;
    private Bot bot;
    private bool isBotEnabled;
    private Player botPlayer = Player.White; // Default bot plays as white
    private TimeSpan botTimeLimit = TimeSpan.FromSeconds(1); // Default time limit: 1 second
    private string statusMessage;

    public Game Game
    {
        get => game;
        set
        {
            game = value;
            OnPropertyChanged();
        }
    }
    
    public bool IsBotEnabled
    {
        get => isBotEnabled;
        set
        {
            isBotEnabled = value;
            OnPropertyChanged();
            
            // Create or remove bot based on the setting
            if (isBotEnabled)
            {
                bot = new Bot(BotPlayer, BotTimeLimit);
            }
            else
            {
                bot = null;
            }
            
            // Update status message
            UpdateStatusMessage();
        }
    }
    
    public Player BotPlayer
    {
        get => botPlayer;
        set
        {
            botPlayer = value;
            OnPropertyChanged();
            
            // Update bot if enabled
            if (isBotEnabled && bot != null)
            {
                bot.BotPlayer = botPlayer;
            }
            
            // Update status message
            UpdateStatusMessage();
        }
    }
    
    public TimeSpan BotTimeLimit
    {
        get => botTimeLimit;
        set
        {
            // Ensure time limit is between 100ms and 10s
            int milliseconds = Math.Max(100, Math.Min(10000, (int)value.TotalMilliseconds));
            botTimeLimit = TimeSpan.FromMilliseconds(milliseconds);
            OnPropertyChanged();
            
            // Update bot if enabled
            if (isBotEnabled && bot != null)
            {
                bot.TimeLimit = botTimeLimit;
            }
        }
    }
    
    // Property for binding to the numeric input in milliseconds
    public int BotTimeLimitMs
    {
        get => (int)botTimeLimit.TotalMilliseconds;
        set
        {
            BotTimeLimit = TimeSpan.FromMilliseconds(value);
            OnPropertyChanged();
        }
    }
    
    public string StatusMessage
    {
        get => statusMessage;
        set
        {
            statusMessage = value;
            OnPropertyChanged();
        }
    }
    
    public ICommand NewGameCommand { get; }
    public ICommand MakeBotMoveCommand { get; }
    
    public MainWindowViewModel()
    {
        Game = new Game();
        UpdateStatusMessage();
        
        // Initialize commands
        NewGameCommand = new RelayCommand(
            param => StartNewGame(),
            param => true
        );
        
        MakeBotMoveCommand = new RelayCommand(
            param => MakeBotMove(),
            param => CanMakeBotMove()
        );
    }
    
    private void StartNewGame()
    {
        Game = new Game();
        UpdateStatusMessage();
    }
    
    private bool CanMakeBotMove()
    {
        return isBotEnabled && 
               !Game.IsGameOver && 
               Game.CurrentPlayer == BotPlayer;
    }
    
    private void MakeBotMove()
    {
        if (!CanMakeBotMove()) return;
        
        // Get and make the bot's move
        Move bestMove = bot.GetBestMove(Game);
        if (bestMove != null)
        {
            Game.MakeMove(bestMove);
            
            // Force UI update
            OnPropertyChanged(nameof(Game));
        }
        
        UpdateStatusMessage();
    }
    
    // Call this method after a player makes a move
    public void OnPlayerMoveCompleted()
    {
        UpdateStatusMessage();
        
        // If it's the bot's turn, make a move automatically
        if (IsBotEnabled && Game.CurrentPlayer == BotPlayer && !Game.IsGameOver)
        {
            // Use a small delay to make the bot's move visible to the player
            System.Threading.Tasks.Task.Delay(100).ContinueWith(_ => 
            {
                MakeBotMove();
                
            }, System.Threading.Tasks.TaskScheduler.FromCurrentSynchronizationContext());
        }
    }
    
    private void UpdateStatusMessage()
    {
        if (Game.IsGameOver)
        {
            Player winner = Game.CurrentPlayer == Player.Red ? Player.White : Player.Red;
            StatusMessage = $"Game Over! {winner} wins!";
        }
        else
        {
            string currentPlayerText = Game.CurrentPlayer == Player.Red ? "Red" : "White";
            string botStatus = IsBotEnabled ? $" (Bot plays as {(BotPlayer == Player.Red ? "Red" : "White")})" : "";
            StatusMessage = $"Current Player: {currentPlayerText}{botStatus}";
        }
    }

    // INotifyPropertyChanged implementation.
    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName]string propertyName = null) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}