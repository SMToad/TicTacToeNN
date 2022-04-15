using System;
using System.Linq;
using System.Windows.Controls;
using TicTacToeWPF.GameElements;
using TicTacToeWPF.Models;
using TicTacToeWPF.Players;

namespace TicTacToeWPF
{
    public class Game
    {
        public GameModel GameModel { get; set; }
        public GameState PlayRound()
        {
            GameModel.Players.X.NewGame();
            GameModel.Players.O.NewGame();
            GameModel.CurrentTurn = PlayerTurn.X;
            Player currentPlayer = GameModel.Players.X;
            do
            {
                (int X, int Y) move = currentPlayer.Move(GameModel.PlayBoard, GameModel.CurrentTurn);
                GameModel.PlayBoard.MakeMove(move, (int)GameModel.CurrentTurn);
                GameModel.GameState = GetGameState(move);
                currentPlayer = SwitchTurn();
            } while (GameModel.GameState == GameState.InGame);

            GameModel.Players.X.Reward(GetGameScore(PlayerTurn.X));
            GameModel.Players.O.Reward(GetGameScore(PlayerTurn.O));
           return GameModel.GameState;
        }
        public Player SwitchTurn()
        {
            Player nextPlayer = null;
            switch (GameModel.CurrentTurn)
            {
                case PlayerTurn.X:
                    GameModel.CurrentTurn = PlayerTurn.O;
                    nextPlayer = GameModel.Players.O;
                    break;
                case PlayerTurn.O:
                    GameModel.CurrentTurn = PlayerTurn.X;
                    nextPlayer = GameModel.Players.X;
                    break;
            }
            return nextPlayer;
        }
        public GameState GetGameState((int X, int Y) lastMove)
        {
            int[,] board = GameModel.PlayBoard.Board;
            int size = GameModel.PlayBoard.Size;
            int rowSum = 0, colSum = 0, diagSum = 0;
            for (int i = 0; i < size; i++)
            {
                rowSum += board[lastMove.X, i];
                colSum += board[i, lastMove.Y];
            }
            if(lastMove.X == lastMove.Y)
                for (int i = 0; i < size; i++)
                    diagSum += board[i, i];
            else if(lastMove.Y == size - (lastMove.X + 1))
                for (int i = 0; i < size; i++)
                    diagSum += board[i, size - (i + 1)];
           
            if (rowSum == size || colSum == size || diagSum == size)
            {
                return GameState.XWin;
            }
            else if (rowSum == -size || colSum == -size || diagSum == -size)
            {
                return GameState.OWin;
            }
            else if (GameModel.PlayBoard.AvailableMoves().Count != 0)
            {
                return GameState.InGame;
            }
            else
            {
                return GameState.Tie;
            }
        }
        public float GetGameScore(PlayerTurn playerTurn)
        {
            switch (GameModel.GameState)
            {
                case GameState.XWin: 
                    return playerTurn == PlayerTurn.X? 1f : -1f;
                case GameState.OWin:
                    return playerTurn == PlayerTurn.O ? 1f : -1f;
                case GameState.Tie: 
                    return 0.5f;
                default: return 0f;
            }
        }
        
        
    }
}

