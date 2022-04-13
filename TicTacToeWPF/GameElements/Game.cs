using System;
using System.Linq;
using System.Windows.Controls;
using TicTacToeWPF.GameElements;
using TicTacToeWPF.Players;

namespace TicTacToeWPF
{
    public static class Game
    {
        public static float GetGameScore(GameState gameState)
        {
            switch (gameState)
            {
                case GameState.XWin: return 1;
                case GameState.OWin: return -1;
                default: return 0;
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
        public static GameState Play(PlayBoard playBoard, ref Grid gridPlayBoard, (Player X, Player O) players)
        {
            players.X.NewGame();
            players.O.NewGame();
            PlayerTurn currentTurn = PlayerTurn.X;
            Player currentPlayer = players.X;
            GameState gameState;
            do
            {
                (int X, int Y) move = currentPlayer.Move(playBoard, currentTurn);
                playBoard.MakeMove(move, (int)currentTurn, gridPlayBoard);
                gameState = GetGameState(playBoard, move);
                currentPlayer = SwitchTurn(players, ref currentTurn);
            } while (gameState == GameState.InGame);
            
           players.X.Reward(GetGameScore(gameState));
           players.O.Reward(-1);
           return gameState;
        }
    }
}

