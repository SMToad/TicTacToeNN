using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicTacToeWPF.Architecture;
using TicTacToeWPF.Learning;

namespace TicTacToeWPF.NeuralNetwork
{
    public class Training
    {
        public int TrainSample { get; set; } = 30;
        public BackPropagation BackPropagation { get; set; }
        public MemoryBuffer MemoryBuffer { get; set; }
        public Training(Network network)
        {
            BackPropagation = new BackPropagation(network);
            MemoryBuffer = new MemoryBuffer();
        }
        public void Train(double[] input, double[] tarqetQValues)
        {
            BackPropagation.Run(input, tarqetQValues);
        }
        public void EmptyBuffer()
        {
            MemoryBuffer = new MemoryBuffer();
        }
        public MemoryBuffer GetTrainBuffer()
        {
            MemoryBuffer trainBuffer = new MemoryBuffer();
            trainBuffer.Add(MemoryBuffer.GetSample(TrainSample));
            return trainBuffer;
        }
    }
}
