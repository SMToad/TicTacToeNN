using System;
using System.Collections.Generic;
using System.Text;
using TicTacToeWPF.GameElements;

namespace TicTacToeWPF.Players
{
    public abstract class Player
    {
        public virtual void NewGame() { }
        public virtual void Reward(float rewardValue) { }
        public abstract void Move(PlayBoard playBoard, PlayerTurn currentTurn);
    }
}
