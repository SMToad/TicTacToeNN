using System;
using System.Collections.Generic;
using System.Linq;
using TicTacToeWPF.NeuralNetwork;
using TicTacToeWPF.Helpers;
using TicTacToeWPF.Learning;
using TicTacToeWPF.GameElements;

namespace TicTacToeWPF.Players
{
    public class Agent : Player
    {
        public QLearning QLearning { get; set; }
        public Training Training { get; set; }
        public bool InTraining { get; set; }
        public Agent(int boardSize = 3)
        {
            QLearning = new QLearning(boardSize);
            Training = new Training(QLearning.Network);
            InTraining = true;
        }
        public void ToggleTraining()
        {
            InTraining = !InTraining;
        }
        public override void NewGame()
        {
            if (InTraining)
                QLearning.CloneNetwork();
            Training.EmptyBuffer();
        }
        public override void Move(PlayBoard playBoard, PlayerTurn currentTurn)
        {
            if (currentTurn == PlayerTurn.O) playBoard.InvertValues();
            LastMove = QLearning.PredictMove(playBoard);
            Training.MemoryBuffer.Add(new MemoryEntry(playBoard.Size)
            {
                Move = LastMove,
                State = playBoard.GetBoardArray()
            });
            if (currentTurn == PlayerTurn.O) playBoard.InvertValues();
        }
        public override void Reward(float rewardValue)
        {
            Training.MemoryBuffer.AddRewardData(rewardValue);
            if (!InTraining) return;
            MemoryBuffer trainBuffer = Training.GetTrainBuffer();

            for (int i = 0; i < trainBuffer.Size - 1; i++)
            {
                double[] currState = trainBuffer.History[i].State.ToDouble();
                Training.Train(currState, QLearning.GetTargetQValues(trainBuffer.History[i]));
            }
           QLearning.DecreaseEpsilon();

        }
    }
}
