using System;
using System.Collections.Generic;
using System.Diagnostics;
using NeuralNetwork.Core;
using NeuralNetwork.ServicesManager;
using NeuralNetwork.ServicesManager.Vectors;

namespace NeuralNetwork
{
    static class Program
    {
        private static NeuralNetwork _net;
        private static FileManager _fileManager;

        private static Vectorizer _vectorizer;

        static void Main(string[] args)
        {
            #region Set process settings

            Process thisProc = Process.GetCurrentProcess();
            thisProc.PriorityClass = ProcessPriorityClass.High;

            #endregion

            const int receptors = 75;
            const int numberOfOutputClasses = 2; // Количество наших классов
            int[] neuronByLayer = { 500, 500, numberOfOutputClasses };
            _fileManager = new FileManager("memory.txt");
            _net = new NeuralNetwork(neuronByLayer, receptors, _fileManager);

            var networkTeacher = new NetworkTeacher(_net, _fileManager)
            {
                Iteration = 100,
                TestVectors = _fileManager.ReadVectors("inputDataTest3.txt")
            };

            //Vectorize();

            //networkTeacher.PreparingLearningData();

            networkTeacher.TrainNet();

            Console.ReadKey();
        }

        private static void Vectorize()
        {
            string trainDataFolder = "data";
            string outputDataFolder = "vectorizedData";

            _vectorizer = new Vectorizer();
            _vectorizer.Vectorizing(trainDataFolder, outputDataFolder);

            Console.WriteLine("Vectorizing done!");
        }
    }
}
