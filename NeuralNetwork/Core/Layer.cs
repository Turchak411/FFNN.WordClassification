using System.Collections.Generic;
using NeuralNetwork.ServicesManager;

namespace NeuralNetwork.Core
{
    public class Layer
    {
        private List<Neuron> _neuronList = new List<Neuron>();

        private Layer() { }

        public Layer(int neuronCount, int weightCount, int layerNumber, FileManager fileManager)
        {
            double offsetValue = 0.5;

            for (int i = 0; i < neuronCount; i++)
            {
                double[] weights = fileManager.LoadMemory(layerNumber, i);
                Neuron neuron = new Neuron(weights, offsetValue, -1, 0.3);

                _neuronList.Add(neuron);
            }
        }

        public double[] Handle(double[] data)
        {
            double[] layerResultVector = new double[_neuronList.Count];

            for (int i = 0; i < layerResultVector.Length; i++)
            {
                layerResultVector[i] = _neuronList[i].Handle(data);
            }

            return layerResultVector;
        }

        // CALCULATING ERRORS:

        public void CalcErrorAsOut(double[] rightAnwsersSet)
        {
            for (int i = 0; i < _neuronList.Count; i++)
            {
                _neuronList[i].CalcErrorForOutNeuron(rightAnwsersSet[i]);
            }
        }

        public void CalcErrorAsHidden(double[][] nextLayerWeights, double[] nextLayerErrors)
        {
            for (int i = 0; i < _neuronList.Count; i++)
            {
                _neuronList[i].CalcErrorForHiddenNeuron(i, nextLayerWeights, nextLayerErrors);
            }
        }

        // CHANGE WEIGHTS:

        public void ChangeWeights(double learnSpeed, double[] anwsersFromPrewLayer)
        {
            for (int i = 0; i < _neuronList.Count; i++)
            {
                _neuronList[i].ChangeWeights(learnSpeed, anwsersFromPrewLayer);
            }
        }

        public double[] GetLastAnwsers()
        {
            double[] lastAnwsers = new double[_neuronList.Count];

            for (int i = 0; i < _neuronList.Count; i++)
            {
                lastAnwsers[i] = _neuronList[i].GetLastAnwser();
            }

            return lastAnwsers;
        }

        public double[][] GetWeights()
        {
            double[][] weights = new double[_neuronList.Count][];

            for (int i = 0; i < _neuronList.Count; i++)
            {
                weights[i] = _neuronList[i].GetWeights();
            }

            return weights;
        }

        public double[] GetErrors()
        {
            double[] errors = new double[_neuronList.Count];

            for (int i = 0; i < _neuronList.Count; i++)
            {
                errors[i] = _neuronList[i].GetError();
            }

            return errors;
        }

        // SAVE MEMORY:

        public void SaveMemory(FileManager fileManager, int layerNumber)
        {
            for (int i = 0; i < _neuronList.Count; i++)
            {
                _neuronList[i].SaveMemory(fileManager, layerNumber, i);
            }
        }
    }
}
