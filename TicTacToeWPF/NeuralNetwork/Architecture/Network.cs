using System;
using TicTacToeWPF.NeuralNetwork;

namespace TicTacToeWPF.Architecture
{
    public class Network
    {
        public int InputsCount { get; protected set; }

        protected int layersCount;
        public Layer[] Layers { get; protected set; }
        public double[] Output { get; protected set; }
        public Network(ActivationFunction function, int inputsCount, params int[] neuronsCount)
        {
            this.InputsCount = Math.Max(1, inputsCount);
            this.layersCount = Math.Max(1, neuronsCount.Length);
            this.Layers = new Layer[this.layersCount];
            for (int i = 0; i < Layers.Length; i++)
            {
                Layers[i] = new Layer(
                    neuronsCount[i],
                    (i == 0) ? inputsCount : neuronsCount[i - 1],
                    function);
            }
        }
        public void Randomize()
        {
            foreach (Layer layer in Layers)
            {
                layer.Randomize();
            }
        }
        public double[] Compute(double[] input)
        {
            double[] output = input;
            for (int i = 0; i < Layers.Length; i++)
            {
                output = Layers[i].Compute(output);
            }
            this.Output = output;
            return output;
        }
        public void Clone(Network net)
        {
            for (int i = 0; i < net.layersCount; i++)
            {
                Layer layer = net.Layers[i];
                for (int j = 0; j < layer.Neurons.Length; j++)
                {
                    Neuron neuron = layer.Neurons[j];
                    Layers[i].Neurons[j].Clone(neuron);
                }
            }
        }
        public void SetActivationFunction(ActivationFunction function)
        {
            for (int i = 0; i < Layers.Length; i++)
            {
                ((Layer)Layers[i]).SetActivationFunction(function);
            }
        }
    }
}
