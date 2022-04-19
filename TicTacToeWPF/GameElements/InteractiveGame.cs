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
            GameModel.Players.First().Move(GameModel.PlayBoard, GameModel.CurrentTurn);
            if(GameModel.Players.First() is Agent)
                EndMove();
        }
        public void EndMove()
        {
            GameModel.PlaceMoveOnBoard(GameModel.Players.First());
            GameModel.GameState = GetGameState();
            if (GameModel.GameState != GameState.InGame)
                RewardPlayers();
            else
            {
                GameModel.SwitchPlayers();
                //await Task.Delay(TimeSpan.FromSeconds(2));
                StartMove();
            }
        }
    }
}
