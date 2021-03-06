using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using TicTacToeWPF.GameElements;
using TicTacToeWPF.Models;
using TicTacToeWPF.Players;
using TicTacToeWPF.Helpers;

namespace TicTacToeWPF
{
    public class Game
    {
        public GameModel GameModel { get; set; }

        public GameState Play()
        {
            GameModel.Players.ForEach(p => p.NewGame());
            do
            {
                Player currentPlayer = GameModel.GetCurrentPlayer();
                currentPlayer.Move(GameModel.PlayBoard, GameModel.CurrentTurn);
                GameModel.GameState = GetGameState();
                GameModel.UpdateCurrentTurn();
            } while (GameModel.GameState == GameState.InGame);
            RewardPlayers();
           return GameModel.GameState;
        }
        public void RewardPlayers()
        {
            GameModel.Players.First().Reward(GetGameScore(PlayerTurn.X));
            GameModel.Players.Last().Reward(GetGameScore(PlayerTurn.O));
        }
        public GameState GetGameState()
        {
            int[,] board = GameModel.PlayBoard.GetBoard();
            List<(int X, int Y)> playedMoves = GameModel.PlayBoard.PlayedMoves;
            int size = GameModel.PlayBoard.Size;
            (int X, int Y) lastMove = playedMoves.Last();
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
            float score = GameModel.GameState.ToFloat();
            return Math.Abs(score) == 1? score * (int)playerTurn : score;
        }
    }
}

