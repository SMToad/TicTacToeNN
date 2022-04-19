using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToeWPF.Helpers
{
    public class MemoryEntry
    {
        private int _boardSize;
        public (int X, int Y) Move { get; set; }
        private int[] _state;
        public int[] State { get => _state; set => _state = value.Clone() as int[]; }
        private int[] _nextState;
        public int[] NextState 
        { 
            get => _nextState; 
            set => _nextState = value?.Clone() as int[]; 
        }
        public float Reward { get; set; }
        public MemoryEntry(int boardsize)
        {
            _boardSize = boardsize;
        }
        public int GetMoveIndex()
        {
            return Move.X * _boardSize + Move.Y;
        }
    }
}
