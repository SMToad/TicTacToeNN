using System;
using System.Collections.Generic;
using System.Text;
using TicTacToeWPF.GameElements;

namespace TicTacToeWPF.Players
{
    class BoringPlayer: Player
    {
        public override (int X, int Y) Move(PlayBoard playBoard, PlayerTurn currentTurn) => playBoard.AvailableMoves()[0];
        
    }
}
