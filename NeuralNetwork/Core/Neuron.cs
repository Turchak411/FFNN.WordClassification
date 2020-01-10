using System;
using NeuralNetwork.ServicesManager;

namespace NeuralNetwork.Core
{
    public class Neuron
    {
        private double[] _weights;
        private double _offsetValue;
        private double _offsetWeight;

        private double _lastAnwser;

        private double _error;

        private Neuron() { }

        public Neuron(double[] weightsValues, double offsetValue, double offsetWeight, double activationThreshold)
        {
            _weights = weightsValues;
            _offsetValue = offsetValue;
            _offsetWeight = offsetWeight;

            _error = 1;
        }

        public double Handle(double[] data)
        {
            double x = CalcSum(data);
            double actFunc = ActivationFunction(x);

            _lastAnwser = actFunc;
            return actFunc;
        }

        private double CalcSum(double[] data)
        {
            double x = 0;

            for (int i = 0; i < _weights.Length; i++)
            {
                x += _weights[i] * data[i];
            }

            return x + _offsetValue * _offsetWeight;
        }

        private double ActivationFunction(double x)
        {
            return 1 / (1 + Math.Pow(Math.E, -x));     // Sigmoid-func
        }

        // CALCULATING ERRORS:

        public void CalcErrorForOutNeuron(double rightAnwser)
        {
            _error = (rightAnwser - _lastAnwser) * _lastAnwser * (1 - _lastAnwser);
        }

        public double CalcErrorForHiddenNeuron(int neuronIndex, double[][] nextLayerWeights, double[] nextLayerErrors)
        {
            // Вычисление производной активационной функции:
            _error = _lastAnwser * (1 - _lastAnwser);

            // Суммирование ошибок со следующего слоя:
            double sum = 0;

            for (int i = 0; i < nextLayerWeights.GetLength(0); i++)
            {
                sum += nextLayerWeights[i][neuronIndex] * nextLayerErrors[i];
            }

            _error = _error * sum;

            return _error;
        }

        public double[] GetWeights()
        {
            return _weights;
        }

        public double GetError()
        {
            return _error;
        }

        // CHANGE WEIGHTS:

        public void ChangeWeights(double learnSpeed, double[] anwsersFromPrewLayer)
        {
            for (int i = 0; i < _weights.Length; i++)
            {
                _weights[i] = _weights[i] + learnSpeed * _error * anwsersFromPrewLayer[i];
            }

            // Изменение величины смещения:
            _offsetWeight = _offsetWeight + learnSpeed * _error;
        }

        public double GetLastAnwser()
        {
            return _lastAnwser;
        }

        // SAVE MEMORY:

        public void SaveMemory(FileManager fileManager, int layerNumber, int neuronNumber)
        {
            fileManager.SaveMemory(layerNumber, neuronNumber, _weights);
        }

        public void SaveMemory(FileManager fileManager, int layerNumber, int neuronNumber, string memoryPath)
        {
            fileManager.SaveMemory(layerNumber, neuronNumber, _weights, memoryPath);
        }
    }
}
