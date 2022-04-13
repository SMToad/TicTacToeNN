using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicTacToeWPF.GameElements;

namespace TicTacToeWPF.Models
{
    public class TrainStats
    {
        public int RoundsCount { get; set; }
        public int WinsCount { get; set; }
        public int LossesCount { get; set; }
        public int TiesCount { get; set; }
        public void ClearResults()
        {
            WinsCount = 0;
            LossesCount = 0;
            TiesCount = 0;
        }
        public void Clear()
        {
            RoundsCount = 0;
            ClearResults();
        }
        public void Increment(GameState gameState)
        {
            switch (gameState)
            {
                case GameState.XWin:
                    if (RoundsCount % 2 == 0)
                        WinsCount++;
                    else LossesCount++;
                    break;
                case GameState.OWin:
                    if (RoundsCount % 2 == 0)
                        LossesCount++;
                    else WinsCount++;
                    break;
                case GameState.Tie:
                    TiesCount++;
                    break;
            }
            RoundsCount++;
        }
    }
}
