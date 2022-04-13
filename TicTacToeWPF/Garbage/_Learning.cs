//using System;
//using TicTacToeWPF.NeuralNetwork;

//namespace TicTacToeWPF.Learning
//{
//    public class _Learning
//    {
//        private Network network;
//        private double learningRate = 0.025;
//        public double LearningRate
//        {
//            get { return learningRate; }
//            set
//            {
//                learningRate = Math.Max(0.0, Math.Min(1.0, value));
//            }
//        }
//        private double momentum = 0.01;
//        public double Momentum
//        {
//            get { return momentum; }
//            set
//            {
//                momentum = Math.Max(0.0, Math.Min(1.0, value));
//            }
//        }

//        private double[][] signals = null;
//        private double[][][] weightsUpdates = null;
//        private double[][][] prevWeightsDelta = null;
//        private double[][][] wGrads = null;
//        private double[][] biasUpdates = null;
//        private double[][] prevBiasDelta = null;
//        private double[][] bGrads = null;

//        public _Learning(Network network)
//        {
//            this.network = network;
//            signals = new double[network.Layers.Length][];
//            weightsUpdates = new double[network.Layers.Length][][];
//            prevWeightsDelta = new double[network.Layers.Length][][];
//            wGrads = new double[network.Layers.Length][][];
//            biasUpdates = new double[network.Layers.Length][];
//            prevBiasDelta = new double[network.Layers.Length][];
//            bGrads = new double[network.Layers.Length][];

//            for (int i = 0; i < network.Layers.Length; i++)
//            {
//                Layer layer = network.Layers[i];
//                signals[i] = new double[layer.Neurons.Length];
//                weightsUpdates[i] = new double[layer.Neurons.Length][];
//                prevWeightsDelta[i] = new double[layer.Neurons.Length][];
//                wGrads[i] = new double[layer.Neurons.Length][];
//                biasUpdates[i] = new double[layer.Neurons.Length];
//                prevBiasDelta[i] = new double[layer.Neurons.Length];
//                bGrads[i] = new double[layer.Neurons.Length];

//                for (int j = 0; j < weightsUpdates[i].Length; j++)
//                {
//                    weightsUpdates[i][j] = new double[layer.InputsCount];
//                    prevWeightsDelta[i][j] = new double[layer.InputsCount];
//                    wGrads[i][j] = new double[layer.InputsCount];
//                }
//            }
//        }

//        public double Run(double[] input, double[] output)
//        {
//            double[]res=network.Compute(input);
//           double error= CalculateError(output);
//            CalculateUpdates(input);
//            UpdateNetwork();
//            return error;
//        }

//        public double RunEpoch(double[][] input, double[][] output)
//        {
//            double error = 0.0;
//            for (int i = 0; i < input.Length; i++)
//            {
//                error += Run(input[i], output[i]);
//            }
//            return error;
//        }

//        private double CalculateError(double[] desiredOutput)
//        {
//            Layer layer, layerNext;
//            double[] errors, errorsNext, bGrad;
//            double[][] wGrad;
//            double error = 0, e, sum;
//            double output;
//            int layersCount = network.Layers.Length;

//            ActivationFunction function = (network.Layers[0].Neurons[0] as Neuron).ActivationFunction;

//            layer = network.Layers[layersCount - 1];
//            errors = signals[layersCount - 1];

//            for (int i = 0; i < layer.Neurons.Length; i++)
//            {
//                output = layer.Neurons[i].Output;
//                e = desiredOutput[i] - output;
//                errors[i] = e * function.Derivative(output);
//                error += (e * e);
//            }
//            error /= layer.Neurons.Length;
//            for (int j = layersCount - 2; j >= 0; j--)
//            {
//                layer = network.Layers[j];
//                layerNext = network.Layers[j + 1];
//                errors = signals[j];
//                errorsNext = signals[j + 1];
//                bGrad = bGrads[j];
//                wGrad = wGrads[j];

//                for (int i = 0; i < layer.Neurons.Length; i++)
//                {
//                    sum = 0.0;
//                    for (int k = 0; k < layerNext.Neurons.Length; k++)
//                    {
//                        sum += errorsNext[k] * layer.Neurons[i].Weights[k];
//                        wGrad[i][k] = errorsNext[k] * layer.Neurons[i].Output;
//                        bGrad[k] = errorsNext[k];
//                    }
//                    errors[i] = sum * function.Derivative(layer.Neurons[i].Output);
//                }
//            }
//            return error;
//        }

//        private void CalculateUpdates(double[] input)
//        {
//            Layer layer, layerNext;
//            double[][] layerWeightsUpdates;
//            double[][] wGrad;
//            double[][] prevWDelta;
//            double[] layerBiasUpdates;
//            double[] bGrad;
//            double[] prevBDelta;
//            for (int k = 0; k < network.Layers.Length - 1; k++)
//            {
//                layer = network.Layers[k];
//                layerNext = network.Layers[k + 1];
//                layerWeightsUpdates = weightsUpdates[k];
//                wGrad = wGrads[k];
//                prevWDelta = prevWeightsDelta[k];
//                layerBiasUpdates = biasUpdates[k];
//                bGrad = bGrads[k];
//                prevBDelta = prevBiasDelta[k];

//                for (int i = 0; i < layer.Neurons.Length; i++)
//                {
//                    for (int j = 0; j < layerNext.Neurons.Length; j++)
//                    {
//                        double delta = wGrad[i][j] * learningRate;
//                        layerWeightsUpdates[i][j] += delta + prevWDelta[i][j] * momentum;
//                        prevWDelta[i][j] = delta;

//                        delta = bGrad[j] * learningRate;
//                        layerBiasUpdates[j] += delta + prevBDelta[j] * momentum;
//                        prevBDelta[j] = delta;
//                    }
//                }
//            }
//        }

//        public void UpdateNetwork()
//        {
//            Neuron neuron;
//            Layer layer;
//            double[][] layerWeightsUpdates;
//            double[] layerBiasUpdates;
//            double[] neuronWeightUpdates;

//            for (int i = 0; i < network.Layers.Length; i++)
//            {
//                layer = network.Layers[i];
//                layerWeightsUpdates = weightsUpdates[i];
//                layerBiasUpdates = biasUpdates[i];

//                for (int j = 0; j < layer.Neurons.Length; j++)
//                {
//                    neuron = layer.Neurons[j] as Neuron;
//                    neuronWeightUpdates = layerWeightsUpdates[j];

//                    for (int k = 0; k < neuron.Weights.Length; k++)
//                    {
//                        neuron.Weights[k] += neuronWeightUpdates[k];
//                    }
//                    neuron.Bias += layerBiasUpdates[j];
//                }
//            }
//        }
//    }
//}
