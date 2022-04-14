using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using TicTacToeWPF.Players;

namespace TicTacToeWPF.GameElements
{
    public static class InteractiveGame
    {
        public static PlayerTurn PlayerTurn { get; set; }
        public static Grid GridPlayBoard { get; set; }
        public static void SwitchTurn()
        {
            switch (PlayerTurn)
            {
                case PlayerTurn.X:
                    PlayerTurn = PlayerTurn.O;
                    break;
                case PlayerTurn.O:
                    PlayerTurn = PlayerTurn.X;
                    break;
            }
        }
        public static void StartGame(PlayBoard playBoard, (Player X, Player O) players)
        {
            players.X?.NewGame();
            players.O?.NewGame();
            PlayerTurn = PlayerTurn.X;
            Player currentPlayer = players.X;
            if (currentPlayer != null)
                StartMove(playBoard, currentPlayer);
        }
        public static void StartMove(PlayBoard playBoard, Player currentPlayer)
        {
            (int X, int Y) move = currentPlayer.Move(playBoard, PlayerTurn);
            Button moveBtn = null;
            foreach (Button btn in GridPlayBoard.Children)
                if (Grid.GetRow(btn) == move.X && Grid.GetColumn(btn) == move.Y)
                {
                    moveBtn = btn;
                    break;
                }
            EndMove(playBoard, move, ref moveBtn);
        }
        public static void EndMove(PlayBoard playBoard, (int X, int Y) move, ref Button moveBtn, Player currentPlayer = null)
        {
            playBoard.MakeMove(move, (int)PlayerTurn);
            moveBtn.Content = PlayerTurn;
            GameState gameState = Game.GetGameState(playBoard, move);
            if (gameState != GameState.InGame)
                EndGame();
            else
            {
                SwitchTurn();
                if (currentPlayer != null)
                    StartMove(playBoard, currentPlayer);
            }
        }
        public static void EndGame()
        {

        }
    }
}
