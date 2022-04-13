using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicTacToeWPF.Players;
using TicTacToeWPF.GameElements;
using System.Windows.Threading;

namespace TicTacToeWPF.Models
{
    public class AppViewModel
    {
        public PlayBoard PlayBoard { get; set; }
        public Agent Agent { get; set; }
        public Player Opponent { get; set; }
        public DispatcherTimer Timer { get; set; }
        public int TrainRounds { get; set; }
        public int TotalRounds { get; set; }
    }
}
