using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicTacToeWPF.GameElements;

namespace TicTacToeWPF.Players
{
    public class Person: Player
    {
        public string StatusBarLabel { get; set; }
        public override void NewGame()
        {
            StatusBarLabel = "Игра против сети. ";
        }
        public override void Move(PlayBoard playBoard, PlayerTurn currentTurn)
        {
            StatusBarLabel += "Ваш ход. ";
            
        }
        public override void Reward(float rewardValue)
        {
            switch (rewardValue)
            {
                case 1f:
                    StatusBarLabel = "Поздравляем, Вы победили!";
                    break;
                case -1f:
                    StatusBarLabel = "Увы, Вы проиграли.";
                    break;
                case 0.5f:
                    StatusBarLabel = "Ничья!";
                    break;
            }
        }
    }
}
