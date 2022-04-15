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
        public Grid GridPlayBoard { get; set; }
        public new void SwitchTurn()
        {
            GameModel.CurrentTurn = GameModel.CurrentTurn == PlayerTurn.X ? PlayerTurn.O : PlayerTurn.X;
        }
        public Player GetCurrentPlayer()
        {
            return GameModel.CurrentTurn == PlayerTurn.X? GameModel.Players.X: GameModel.Players.O;
        }
        public void StartGame()
        {
            foreach (Button btn in GridPlayBoard.Children)
                btn.Content = "";
            GameModel.PlayBoard = new PlayBoard(GameModel.PlayBoard.Size);
            GameModel.Players.X.NewGame();
            GameModel.Players.O.NewGame();
            GameModel.CurrentTurn = PlayerTurn.X;
            StartMove();
        }
        public void StartMove()
        {
            (int X, int Y) move = GetCurrentPlayer().Move(GameModel.PlayBoard, GameModel.CurrentTurn);
            if (GetCurrentPlayer() is Person) return;
            Button moveBtn = null;
            foreach (Button btn in GridPlayBoard.Children)
                if (Grid.GetRow(btn) == move.X && Grid.GetColumn(btn) == move.Y)
                {
                    moveBtn = btn;
                    break;
                }
            EndMove(move, ref moveBtn);
        }
        public void EndMove((int X, int Y) move, ref Button moveBtn)
        {
            GameModel.PlayBoard.MakeMove(move, (int)GameModel.CurrentTurn);
            moveBtn.Content = GameModel.CurrentTurn;
            GameModel.GameState = GetGameState(move);
            if (GameModel.GameState != GameState.InGame)
                EndGame();
            else
            {
                SwitchTurn();
                StartMove();
            }
        }
        public void EndGame()
        {
            GameModel.Players.X.Reward(GetGameScore(PlayerTurn.X));
            GameModel.Players.O.Reward(GetGameScore(PlayerTurn.O));
            foreach (Button btn in GridPlayBoard.Children)
                btn.IsHitTestVisible = false;
            //UpdateVisualStats(gameState);
            GameModel.Players = (GameModel.Players.O, GameModel.Players.X);
            //SwitchTurn();
            //StartGame();
        }
    }
}
