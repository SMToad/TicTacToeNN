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
        public Person Person { get; set; }
        public Player TrainOpponent { get; set; }
        public int TrainRounds { get; set; }
        public int TotalRounds { get; set; }
    }
}
