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
        public override (int X, int Y) Move(PlayBoard playBoard, PlayerTurn currentTurn)
        {
            if (currentTurn == PlayerTurn.O) playBoard.Invert();
            (int X, int Y) move = QLearning.PredictMove(playBoard);
            if (currentTurn == PlayerTurn.O) playBoard.Invert();
            return move;
        }
        public override void Reward(float rewardValue)
        {
            Training.MemoryBuffer.AddEndGameData(rewardValue);
            if (!InTraining) return;
            MemoryBuffer trainBuffer = Training.GetTrainBuffer();

            for (int i = 0; i < trainBuffer.Size - 1; i++)
            {
                double[] currState = trainBuffer.History[i].Board.FlattenDouble();
                Training.Train(currState, QLearning.GetTargetQValues(trainBuffer.History[i], rewardValue));
            }
           QLearning.DecreaseEpsilon();

        }
    }
}
