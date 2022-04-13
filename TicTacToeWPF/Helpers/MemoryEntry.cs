using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToeWPF.Helpers
{
    public class MemoryEntry
    {
        public (int X, int Y) Move { get; set; }
        public int[,] Board { get; set; }
        public int[,] NextState { get; set; }
        public float Reward { get; set; }
    }
}
