using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using TicTacToeWPF.GameElements;

namespace TicTacToeWPF.Players
{
    public class Person: Player
    {
        public Label LbStatusBar { get; set; }
        public Person(Label lbStatusBar)
        {
            LbStatusBar = lbStatusBar;
        }
        public override void NewGame() 
        {
            LbStatusBar.Content += "Ваш ход. ";
        }
        public override void Reward(float rewardValue) 
        {
            switch (rewardValue)
            {
                case 1f:
                    LbStatusBar.Content = "Вы победили! ";
                    break;
                case -1f:
                    LbStatusBar.Content = "Сеть победила! ";
                    break;
                case 0.5f:
                    LbStatusBar.Content = "Ничья. ";
                    break;
            }
        }
        public override (int X, int Y) Move(PlayBoard playBoard, PlayerTurn currentTurn) 
        {
            LbStatusBar.Content = "Сеть приняла решение. Ваш ход. ";
            return (-1, -1);
        }
    }
}
