using System;
using TicTacToeWPF.Architecture;
using TicTacToeWPF.NeuralNetwork;

namespace TicTacToeWPF.Learning
{
    public class BackPropagation
    {
        private Network _network;
        private double _learningRate = 0.025;
        public double LearningRate
        {
            get { return _learningRate; }
            set
            {
                _learningRate = Math.Max(0.0, Math.Min(1.0, value));
            }
        }

        private double _momentum = 0.01;
        public double Momentum
        {
            get { return _momentum; }
            set
            {
                _momentum = Math.Max(0.0, Math.Min(1.0, value));
            }
        }

        private double[][] _neuronErrors = null;
        private double[][][] _weightsUpdates = null;
        private double[][] _biasUpdates = null;

        public BackPropagation(Network network)
        {
            this._network = network;

            _neuronErrors = new double[network.Layers.Length][];
            _weightsUpdates = new double[network.Layers.Length][][];
            _biasUpdates = new double[network.Layers.Length][];

            for (int i = 0; i < network.Layers.Length; i++)
            {
                Layer layer = network.Layers[i];

                _neuronErrors[i] = new double[layer.Neurons.Length];
                _weightsUpdates[i] = new double[layer.Neurons.Length][];
                _biasUpdates[i] = new double[layer.Neurons.Length];

                for (int j = 0; j < _weightsUpdates[i].Length; j++)
                {
                    _weightsUpdates[i][j] = new double[layer.InputsCount];
                }
            }
        }

        public double Run(double[] input, double[] output)
        {
            _network.Compute(input);
            double error = CalculateError(output);

            CalculateUpdates(input);
            UpdateNetwork();
           
            return error;
        }

        private double CalculateError(double[] desiredOutput)
        {
            Layer layer, layerNext;
            double[] errors, errorsNext;
            double error = 0, e, sum;
            double output;
            int layersCount = _network.Layers.Length;

            ActivationFunction function = (_network.Layers[0].Neurons[0] as Neuron).ActivationFunction;

            layer = _network.Layers[layersCount - 1];
            errors = _neuronErrors[layersCount - 1];

            for (int i = 0; i < layer.Neurons.Length; i++)
            {
                output = layer.Neurons[i].Output;
                e = desiredOutput[i] - output;
                errors[i] = e * function.Derivative(output);
                error += (e * e);
            }
            error /= layer.Neurons.Length;
            for (int j = layersCount - 2; j >= 0; j--)
            {
                layer = _network.Layers[j];
                layerNext = _network.Layers[j + 1];
                errors = _neuronErrors[j];
                errorsNext = _neuronErrors[j + 1];

                for (int i = 0; i < layer.Neurons.Length; i++)
                {
                    sum = 0.0;
                    for (int k = 0; k < layerNext.Neurons.Length; k++)
                    {
                        sum += errorsNext[k] * layerNext.Neurons[k].Weights[i];
                    }
                    errors[i] = sum * function.Derivative(layer.Neurons[i].Output);
                }
            }
            return error;
        }

        private void CalculateUpdates(double[] input)
        {
            Layer layer, layerPrev;
            double[][] layerWeightsUpdates;
            double[] layerBiasUpdates;
            double[] errors;
            double[] neuronWeightUpdates;

            layer = _network.Layers[0];
            errors = _neuronErrors[0];
            layerWeightsUpdates = _weightsUpdates[0];
            layerBiasUpdates = _biasUpdates[0];

            double cachedMomentum =  _learningRate * _momentum;
            double cached1mMomentum = _learningRate * (1 - _momentum);
            double cachedError;

            for (int i = 0; i < layer.Neurons.Length; i++)
            {
                cachedError = errors[i] * cached1mMomentum;
                neuronWeightUpdates = layerWeightsUpdates[i];

                for (int j = 0; j < neuronWeightUpdates.Length; j++)
                {
                    neuronWeightUpdates[j] = cachedMomentum * neuronWeightUpdates[j] + cachedError * input[j];
                }

                layerBiasUpdates[i] = cachedMomentum * layerBiasUpdates[i] + cachedError;
            }

            for (int k = 1; k < _network.Layers.Length; k++)
            {
                layerPrev = _network.Layers[k - 1];
                layer = _network.Layers[k];
                errors = _neuronErrors[k];
                layerWeightsUpdates = _weightsUpdates[k];
                layerBiasUpdates = _biasUpdates[k];

                for (int i = 0; i < layer.Neurons.Length; i++)
                {
                    cachedError = errors[i] * cached1mMomentum;
                    neuronWeightUpdates = layerWeightsUpdates[i];

                    for (int j = 0; j < neuronWeightUpdates.Length; j++)
                    {
                        neuronWeightUpdates[j] = _momentum * neuronWeightUpdates[j] + cachedError * layerPrev.Neurons[j].Output;
                    }

                    layerBiasUpdates[i] = cachedMomentum * layerBiasUpdates[i] + cachedError;
                }
            }
        }

        public void UpdateNetwork()
        {
            Neuron neuron;
            Layer layer;
            double[][] layerWeightsUpdates;
            double[] layerBiasUpdates;
            double[] neuronWeightUpdates;

            for (int i = 0; i < _network.Layers.Length; i++)
            {
                layer = _network.Layers[i];
                layerWeightsUpdates = _weightsUpdates[i];
                layerBiasUpdates = _biasUpdates[i];

                for (int j = 0; j < layer.Neurons.Length; j++)
                {
                    neuron = layer.Neurons[j] as Neuron;
                    neuronWeightUpdates = layerWeightsUpdates[j];

                    for (int k = 0; k < neuron.Weights.Length; k++)
                    {
                        neuron.Weights[k] += neuronWeightUpdates[k];
                    }
                    neuron.Bias += layerBiasUpdates[j];
                }
            }
        }
    }
}
