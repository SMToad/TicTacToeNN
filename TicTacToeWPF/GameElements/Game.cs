using System;
using System.Linq;
using System.Windows.Controls;
using TicTacToeWPF.GameElements;
using TicTacToeWPF.Players;

namespace TicTacToeWPF
{
    public static class Game
    {
        public static float GetGameScore(PlayerTurn playerTurn, GameState gameState)
        {
            switch (gameState)
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
        public static GameState GetGameState(PlayBoard playBoard, (int X, int Y) lastMove)
        {
            int[,] board = playBoard.Board;
            int size = playBoard.Size;
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
            else if (playBoard.AvailableMoves().Count != 0)
            {
                return GameState.InGame;
            }
            else
            {
                return GameState.Tie;
            }
        }
        public static Player SwitchTurn((Player X, Player O) players, ref PlayerTurn currentTurn)
        {
            Player nextPlayer = null;
            switch (currentTurn)
            {
                case PlayerTurn.X:
                    currentTurn = PlayerTurn.O;
                    nextPlayer = players.O;
                    break;
                case PlayerTurn.O:
                    currentTurn = PlayerTurn.X;
                    nextPlayer = players.X;
                    break;
            }
            return nextPlayer;
        }
        public static GameState Play(PlayBoard playBoard, (Player X, Player O) players)
        {
            players.X.NewGame();
            players.O.NewGame();
            PlayerTurn currentTurn = PlayerTurn.X;
            Player currentPlayer = players.X;
            GameState gameState;
            do
            {
                (int X, int Y) move = currentPlayer.Move(playBoard, currentTurn);
                playBoard.MakeMove(move, (int)currentTurn);
                gameState = GetGameState(playBoard, move);
                currentPlayer = SwitchTurn(players, ref currentTurn);
            } while (gameState == GameState.InGame);
            
           players.X.Reward(GetGameScore(PlayerTurn.X, gameState));
           players.O.Reward(GetGameScore(PlayerTurn.O, gameState));
           return gameState;
        }
    }
}

