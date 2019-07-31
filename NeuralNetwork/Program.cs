using System;
using System.Diagnostics;
using NeuralNetwork.Core;
using NeuralNetwork.ServicesManager;
using NeuralNetwork.ServicesManager.Vectors;

namespace NeuralNetwork
{
    static class Program
    {

        static void Main(string[] args)
        {
            #region Set process settings

            Process thisProc = Process.GetCurrentProcess();
            thisProc.PriorityClass = ProcessPriorityClass.High;

            #endregion

            const int receptors = 75;
            const int numberOfOutputClasses = 13; // Количество наших классов
            int[] neuronByLayer = { 45,23, numberOfOutputClasses };


            var teachNetwork = new TeachNetwork(receptors, numberOfOutputClasses, neuronByLayer, "memory.txt")
            {
                Iteration = 10,
                TestVectors = new FileManager().ReadVectors("inputDataTest.txt")
            };

            teachNetwork.TrainNet();

            Console.ReadKey();
        }


        private static Vectorizer _vectorizer;

        private static void Vectorize()
        {
            string trainDataFolder = "data";
            string outputDataFolder = "vectorizedData";

            _vectorizer = new Vectorizer();
            _vectorizer.Vectorizing(trainDataFolder, outputDataFolder);
        }
    }
}
