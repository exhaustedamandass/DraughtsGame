using System;

namespace DraughtsGame.DataModels;

public class Bot
{
    public Player BotPlayer { get; set; }
    public int Depth { get; set; }
    
    public Bot(Player botPlayer, int depth = 3)
    {
        BotPlayer = botPlayer;
        Depth = depth;
    }
    
    /// <summary>
    /// Returns the best move for the bot by running the minimax algorithm.
    /// </summary>
    public Move GetBestMove(Game game)
    {
        var result = Minimax(game, Depth, int.MinValue, int.MaxValue, game.CurrentPlayer);
        return result.move;
    }
    
    /// <summary>
    /// Minimax algorithm with alpha-beta pruning.
    /// Returns a tuple of (score, move) for the given game state.
    /// </summary>
    private (int score, Move move) Minimax(Game game, int depth, int alpha, int beta, Player currentPlayer)
    {
        // Terminal condition: either we've reached the depth limit or the game is over.
        if (depth == 0 || game.IsGameOver)
        {
            return (Evaluate(game), null);
        }
        
        var validMoves = game.Board.GetValidMoves(currentPlayer);
        if (validMoves.Count == 0)
        {
            return (Evaluate(game), null);
        }
        
        Move bestMove = null;
        if (currentPlayer == BotPlayer)
        {
            // Maximizing player: bot.
            int maxEval = int.MinValue;
            foreach (var move in validMoves)
            {
                Game clonedGame = game.Clone();
                clonedGame.MakeMove(move);
                
                // Recurse with the opponent's turn.
                int eval = Minimax(clonedGame, depth - 1, alpha, beta, clonedGame.CurrentPlayer).score;
                if (eval > maxEval)
                {
                    maxEval = eval;
                    bestMove = move;
                }
                
                alpha = Math.Max(alpha, eval);
                if (beta <= alpha)
                    break; // Beta cutoff.
            }
            return (maxEval, bestMove);
        }
        else
        {
            // Minimizing player: opponent.
            int minEval = int.MaxValue;
            foreach (var move in validMoves)
            {
                Game clonedGame = game.Clone();
                clonedGame.MakeMove(move);
                
                int eval = Minimax(clonedGame, depth - 1, alpha, beta, clonedGame.CurrentPlayer).score;
                if (eval < minEval)
                {
                    minEval = eval;
                    bestMove = move;
                }
                
                beta = Math.Min(beta, eval);
                if (beta <= alpha)
                    break; // Alpha cutoff.
            }
            return (minEval, bestMove);
        }
    }
    
    /// <summary>
    /// A simple evaluation function that scores the board based on material.
    /// Each man is worth 1 point and each king is worth 2 points.
    /// </summary>
    private int Evaluate(Game game)
    {
        int score = 0;
        for (int row = 0; row < Board.Size; row++)
        {
            for (int col = 0; col < Board.Size; col++)
            {
                Position pos = new Position(row, col);
                var cell = game.Board.GetCellAt(pos);
                if (!cell.IsEmpty && cell is Piece piece)
                {
                    int pieceValue = piece.IsKing ? 2 : 1;
                    // Score from the bot's perspective.
                    score += (piece.Owner == BotPlayer) ? pieceValue : -pieceValue;
                }
            }
        }
        return score;
    }
}