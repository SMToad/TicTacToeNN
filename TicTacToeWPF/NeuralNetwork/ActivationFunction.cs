using System;

namespace TicTacToeWPF.NeuralNetwork
{
    public class ActivationFunction 
    {
        public double Function(double x) => Math.Tanh(x);

        public double Derivative(double x)
        {
            double y = Function(x);
            return (1 - y * y);
        }

    }
}
