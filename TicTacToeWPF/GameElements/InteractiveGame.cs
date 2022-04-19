using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using TicTacToeWPF.Models;
using TicTacToeWPF.Players;

namespace TicTacToeWPF.GameElements
{
    public class InteractiveGame: Game
    {
        public void StartGame()
        {
            GameModel.PlayBoard = new PlayBoard(GameModel.PlayBoard.Size);
            GameModel.Players.ForEach(p => p.NewGame());
            GameModel.GameState = GameState.InGame;
            StartMove();
        }
        public void StartMove()
        {
            GameModel.GetCurrentPlayer().Move(GameModel.PlayBoard, GameModel.CurrentTurn);
            if(GameModel.GetCurrentPlayer() is Agent)
                EndMove();
        }
        public void EndMove()
        {
            GameModel.GameState = GetGameState();
            if (GameModel.GameState != GameState.InGame)
                RewardPlayers();
            else
            {
                GameModel.UpdateCurrentTurn();
                StartMove();
            }
        }
    }
}
