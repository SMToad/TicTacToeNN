using System;
using System.Collections.Generic;
using TicTacToeWPF.GameElements;

namespace TicTacToeWPF.Players
{
    class RandomPlayer: Player
    {
        public override void Move(PlayBoard playBoard, PlayerTurn currentTurn)
        {
            Random rand = new Random();
            List<(int X, int Y)> moves = playBoard.AvailableMoves();
            int k = rand.Next(moves.Count);
            playBoard.PlaceMove(moves[k], currentTurn);
            
        }
    }
}
