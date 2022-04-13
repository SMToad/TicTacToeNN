using OxyPlot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicTacToeWPF.Helpers;

namespace TicTacToeWPF.Models
{
    public class GraphViewModel
    {
        public List<DataPoint> NetWinsData { get; set; }
        public List<DataPoint> NetLossesData { get; set; }
        public List<DataPoint> TiesData { get; set; }
        public void Clear()
        {
            NetWinsData.Clear();
            NetLossesData.Clear();
            TiesData.Clear();
        }
        public void Add(TrainStats stats, int trainRounds)
        {
            NetWinsData.Add(new DataPoint(stats.RoundsCount, stats.WinsCount.ToPercent(trainRounds)));
            NetLossesData.Add(new DataPoint(stats.RoundsCount, stats.LossesCount.ToPercent(trainRounds)));
            TiesData.Add(new DataPoint(stats.RoundsCount, stats.TiesCount.ToPercent(trainRounds)));
        }
    }
}
