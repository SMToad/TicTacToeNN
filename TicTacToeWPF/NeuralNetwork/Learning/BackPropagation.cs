using System;
using TicTacToeWPF.Architecture;
using TicTacToeWPF.NeuralNetwork;

namespace TicTacToeWPF.Learning
{
    public class BackPropagation
    {
        private Network network;
        private double learningRate = 0.025;
        public double LearningRate
        {
            get { return learningRate; }
            set
            {
                learningRate = Math.Max(0.0, Math.Min(1.0, value));
            }
        }

        private double momentum = 0.01;
        public double Momentum
        {
            get { return momentum; }
            set
            {
                momentum = Math.Max(0.0, Math.Min(1.0, value));
            }
        }

        private double[][] neuronErrors = null;
        private double[][][] weightsUpdates = null;
        private double[][] biasUpdates = null;

        public BackPropagation(Network network)
        {
            this.network = network;

            neuronErrors = new double[network.Layers.Length][];
            weightsUpdates = new double[network.Layers.Length][][];
            biasUpdates = new double[network.Layers.Length][];

            for (int i = 0; i < network.Layers.Length; i++)
            {
                Layer layer = network.Layers[i];

                neuronErrors[i] = new double[layer.Neurons.Length];
                weightsUpdates[i] = new double[layer.Neurons.Length][];
                biasUpdates[i] = new double[layer.Neurons.Length];

                for (int j = 0; j < weightsUpdates[i].Length; j++)
                {
                    weightsUpdates[i][j] = new double[layer.InputsCount];
                }
            }
        }

        public double Run(double[] input, double[] output)
        {
            network.Compute(input);
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
            int layersCount = network.Layers.Length;

            ActivationFunction function = (network.Layers[0].Neurons[0] as Neuron).ActivationFunction;

            layer = network.Layers[layersCount - 1];
            errors = neuronErrors[layersCount - 1];

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
                layer = network.Layers[j];
                layerNext = network.Layers[j + 1];
                errors = neuronErrors[j];
                errorsNext = neuronErrors[j + 1];

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

            layer = network.Layers[0];
            errors = neuronErrors[0];
            layerWeightsUpdates = weightsUpdates[0];
            layerBiasUpdates = biasUpdates[0];

            double cachedMomentum =  learningRate * momentum;
            double cached1mMomentum = learningRate * (1 - momentum);
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

            for (int k = 1; k < network.Layers.Length; k++)
            {
                layerPrev = network.Layers[k - 1];
                layer = network.Layers[k];
                errors = neuronErrors[k];
                layerWeightsUpdates = weightsUpdates[k];
                layerBiasUpdates = biasUpdates[k];

                for (int i = 0; i < layer.Neurons.Length; i++)
                {
                    cachedError = errors[i] * cached1mMomentum;
                    neuronWeightUpdates = layerWeightsUpdates[i];

                    for (int j = 0; j < neuronWeightUpdates.Length; j++)
                    {
                        neuronWeightUpdates[j] = momentum * neuronWeightUpdates[j] + cachedError * layerPrev.Neurons[j].Output;
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

            for (int i = 0; i < network.Layers.Length; i++)
            {
                layer = network.Layers[i];
                layerWeightsUpdates = weightsUpdates[i];
                layerBiasUpdates = biasUpdates[i];

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
