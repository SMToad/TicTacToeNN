using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicTacToeWPF.Architecture;
using TicTacToeWPF.GameElements;
using TicTacToeWPF.Helpers;
using TicTacToeWPF.NeuralNetwork;

namespace TicTacToeWPF.Learning
{
    public class QLearning
    {
        public double Gamma { get; set; } = 0.95;

        public double Epsilon { get; set; } = 0.95;
        public double EpsilonDecrease { get; set; } = 0.95;
        public Network Network { get; private set; }
        public Network TargetNetwork { get; private set; }
        public QLearning(int boardSize)
        {
            int inputLayerSize = boardSize * boardSize,
                hiddenLayerSize = inputLayerSize * boardSize;
            Network = new Network(
                new ActivationFunction(), 
                inputLayerSize, 
                hiddenLayerSize, 
                hiddenLayerSize, 
                inputLayerSize);
            Network.Randomize();
            TargetNetwork = new Network(
                new ActivationFunction(), 
                inputLayerSize, 
                hiddenLayerSize, 
                hiddenLayerSize, 
                inputLayerSize);
            TargetNetwork.Clone(Network);
        }
        public double[] PredictQValues(PlayBoard playBoard) => 
            Network.Compute(playBoard.Board.FlattenDouble());
        public void CloneNetwork() => TargetNetwork.Clone(Network);
        public (int X, int Y) PredictMove(PlayBoard playBoard)
        {
            (int X, int Y) move;
            Random rand = new Random();
            double[] qValues = PredictQValues(playBoard);
            List<int> blockedMoves = playBoard.BlockedMoves();
            List<(int X, int Y)> availableMoves = playBoard.AvailableMoves();
            double[] qValuesCopy = qValues.Clone() as double[];
            for (int i = 0; i < blockedMoves.Count; i++)
                qValuesCopy[blockedMoves[i]] = Double.MinValue;
            double maxQValue = qValuesCopy[0];
            int maxQValueIndex = 0;
            for (int i = 1; i < qValuesCopy.Length; i++)
                if (qValuesCopy[i] > maxQValue)
                {
                    maxQValue = qValuesCopy[i];
                    maxQValueIndex = i;
                }
            if (rand.NextDouble() < Epsilon)
            {
                maxQValueIndex = rand.Next(availableMoves.Count);
                move = availableMoves[maxQValueIndex];
            }
            else
                move = (
                    maxQValueIndex / playBoard.Size,
                    maxQValueIndex % playBoard.Size);
            return move;
        }
        public double[] GetTargetQValues(MemoryEntry memoryEntry, float rewardValue)
        {
            int size = memoryEntry.State.GetLength(0);
            double[] currState = memoryEntry.State.FlattenDouble();
            double[] targetQValues = Network.Compute(currState).Clone() as double[];
            if (memoryEntry.NextState != null)
            {
                double[] nextState = memoryEntry.NextState.FlattenDouble();
                double[] nextQValues = (TargetNetwork.Compute(nextState)).Clone() as double[];
                for (int j = 0; j < nextQValues.Length; j++)
                {
                    if (Math.Abs(nextState[j]) == 1)
                    {
                        nextQValues[j] = Double.MinValue;
                    }
                }
                double maxQValue = Double.MinValue;
                for (int j = 0; j < nextQValues.Length; j++)
                    if (nextQValues[j] > maxQValue) maxQValue = nextQValues[j];
                targetQValues[memoryEntry.Move.X * size + memoryEntry.Move.Y] = maxQValue * Gamma;

                targetQValues[memoryEntry.Move.X * size + memoryEntry.Move.Y] +=  memoryEntry.Reward;
            }
            else
            {
                targetQValues[memoryEntry.Move.X * size + memoryEntry.Move.Y] = rewardValue;
            }
            return targetQValues;
        }
        public void DecreaseEpsilon() => Epsilon *= EpsilonDecrease;
    }
}
