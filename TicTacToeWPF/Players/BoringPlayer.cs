using System;
using System.Collections.Generic;
using System.Text;
using TicTacToeWPF.GameElements;

namespace TicTacToeWPF.Players
{
    class BoringPlayer: Player
    {
        public override void Move(PlayBoard playBoard, PlayerTurn currentTurn)
        {
            LastMove = playBoard.AvailableMoves()[0];
        }
        
    }
}
