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
            playBoard.PlaceMove(playBoard.AvailableMoves()[0], currentTurn);
        }
        
    }
}
