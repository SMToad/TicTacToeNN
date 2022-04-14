using System;
using TicTacToeWPF.NeuralNetwork;

namespace TicTacToeWPF.Architecture
{
    public class Neuron
    {
        public int InputsCount { get; } = 0;
        public double[] Weights { get; set; } = null;
        public double Output { get; protected set; } = 0;
        public double Bias { get; set; } = 0.01;
        public ActivationFunction ActivationFunction { get; set; } = null;
        protected static Random _rand = new Random();
        public static float[] RandRange{get; set;} = { 0.0f, 1.0f };
        public Neuron(int inputs, ActivationFunction function)
        {
            InputsCount = Math.Max(1, inputs);
            Weights = new double[InputsCount];
            this.ActivationFunction = function;
            Randomize();
        }
        public void Randomize()
        {
            double d = RandRange.Length;
            for (int i = 0; i < InputsCount; i++)
                Weights[i] = _rand.NextDouble() * d + RandRange[0];
            Bias = _rand.NextDouble() * (RandRange.Length) + RandRange[0];
        }
        public double Compute(double[] input)
        {
            if (input.Length != InputsCount)
                throw new ArgumentException("Wrong length of the input vector.");
            double sum = 0.0;
            for (int i = 0; i < Weights.Length; i++)
            {
                sum += Weights[i] * input[i];
            }
            sum += Bias;
            double output = ActivationFunction.Function(sum);
            this.Output = output;
            return output;
        }
        public void Clone(Neuron neuron)
        {
            Bias = neuron.Bias;
            Weights = neuron.Weights.Clone() as double[];
        }
    }
}
