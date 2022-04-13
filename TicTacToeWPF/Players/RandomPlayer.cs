using System;
using System.Collections.Generic;
using TicTacToeWPF.GameElements;

namespace TicTacToeWPF.Players
{
    class RandomPlayer: Player
    {
        public override (int X, int Y) Move(PlayBoard playBoard, PlayerTurn currentTurn)
        {
            Random rand = new Random();
            List<(int X, int Y)> moves = playBoard.AvailableMoves();
            int k = rand.Next(moves.Count);
            return moves[k];
        }
    }
}
