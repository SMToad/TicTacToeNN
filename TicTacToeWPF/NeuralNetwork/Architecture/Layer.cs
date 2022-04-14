using System;
using TicTacToeWPF.NeuralNetwork;

namespace TicTacToeWPF.Architecture
{
    public class Layer
    {
        public int InputsCount { get; protected set; } = 0;

        protected int _neuronsCount = 0;
        public Neuron[] Neurons {get; protected set; }
        public double[] Output { get; protected set; }

        public Layer(int neuronsCount, int inputsCount, ActivationFunction function)
        {
            this.InputsCount = Math.Max(1, inputsCount);
            this._neuronsCount = Math.Max(1, neuronsCount);
            Neurons = new Neuron[this._neuronsCount];
            for (int i = 0; i < Neurons.Length; i++)
                Neurons[i] = new Neuron(inputsCount, function);
        }
        public void Randomize()
        {
            foreach (Neuron neuron in Neurons)
                neuron.Randomize();
        }

        public double[] Compute(double[] input)
        {
            double[] output = new double[_neuronsCount];
            for (int i = 0; i < Neurons.Length; i++)
                output[i] = Neurons[i].Compute(input);
            this.Output = output;
            return output;
        }

        public void SetActivationFunction(ActivationFunction function)
        {
            for (int i = 0; i < Neurons.Length; i++)
            {
                ((Neuron)Neurons[i]).ActivationFunction = function;
            }
        }
    }
}
