﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicTacToeWPF.Helpers;

namespace TicTacToeWPF.Models
{
    public class StatsViewModel
    {
        public string RoundsCount { get; set; }
        public string WinsCount { get; set; }
        public string LossesCount { get; set; }
        public string TiesCount { get; set; }
        public StatsViewModel(TrainStats stats, int trainRounds)
        {
            RoundsCount = (stats.RoundsCount/2).ToString();
            WinsCount = stats.WinsCount.ToPercent(trainRounds).ToString() + "%";
            LossesCount = stats.LossesCount.ToPercent(trainRounds).ToString() + "%";
            TiesCount = stats.TiesCount.ToPercent(trainRounds).ToString() + "%";
        }
    }
}
