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
            Network.Compute(playBoard.GetBoardArray().ToDouble());
        public void CloneNetwork() => TargetNetwork.Clone(Network);
        public (int X, int Y) PredictMove(PlayBoard playBoard)
        {
            (int X, int Y) move;
            Random rand = new Random();
            double[] qValues = PredictQValues(playBoard);
            List<(int X, int Y)> availableMoves = playBoard.AvailableMoves();
            int moveIndex = 0;
            if (rand.NextDouble() < Epsilon)
            {
                moveIndex = rand.Next(availableMoves.Count);
                move = availableMoves[moveIndex];
            }
            else
            {
                List<int> blockedMoves = playBoard.BlockedMovesIndexes();
                for (int i = 0; i < blockedMoves.Count; i++)
                    qValues[blockedMoves[i]] = Double.MinValue;
                double maxQValue = qValues[0];
                for (int i = 1; i < qValues.Length; i++)
                    if (qValues[i] > maxQValue)
                    {
                        maxQValue = qValues[i];
                        moveIndex = i;
                    }
                move = (
                    moveIndex / playBoard.Size,
                    moveIndex % playBoard.Size);
            }
            return move;
        }
        public double[] GetTargetQValues(MemoryEntry memoryEntry)
        {
            double[] currState = memoryEntry.State.ToDouble();
            double[] targetQValues = Network.Compute(currState);
            if (memoryEntry.NextState != null)
            {
                double[] nextState = memoryEntry.NextState.ToDouble();
                double[] nextQValues = TargetNetwork.Compute(nextState);
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
                targetQValues[memoryEntry.GetMoveIndex()] = maxQValue * Gamma;

                targetQValues[memoryEntry.GetMoveIndex()] +=  memoryEntry.Reward;
            }
            else
            {
                targetQValues[memoryEntry.GetMoveIndex()] = memoryEntry.Reward;
            }
            return targetQValues;
        }
        public void DecreaseEpsilon() => Epsilon *= EpsilonDecrease;
    }
}
