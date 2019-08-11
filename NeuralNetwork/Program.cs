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

        static void Main(string[] args)
        {
            #region Set process settings

            Process thisProc = Process.GetCurrentProcess();
            thisProc.PriorityClass = ProcessPriorityClass.High;

            #endregion

            const int receptors = 75;
            const int numberOfOutputClasses = 2; // Количество наших классов
            int[] neuronByLayer = { 500, 500, numberOfOutputClasses };
            _fileManager = new FileManager("memory_75_60_50_2.txt");
            _net = new NeuralNetwork(neuronByLayer, receptors, _fileManager);

            var networkTeacher = new NetworkTeacher(_net, _fileManager)
            {
                Iteration = 1000,
                TestVectors = _fileManager.ReadVectors("inputDataTest4.txt")
            };

           // Vectorize();
         //networkTeacher.PreparingLearningData();

           networkTeacher.TrainNet();

            Console.ReadKey();
        }

        /// <summary>
        /// Test neural network
        /// </summary>
        /// <param name="inputVector">Вектор для тестирования</param>
        public static void Test(double[] inputVector)
        {
            var outputVector = _net.Handle(inputVector);

            // Print result vector:
            Console.WriteLine("[Neuron] - [Activated value]");

            for (int i = 0; i < outputVector.Length; i++)
            {
                Console.WriteLine("{0} - {1:f3}", i, outputVector[i]);
            }
        }

        private static Vectorizer _vectorizer;

        private static void Vectorize()
        {
            string trainDataFolder = "DataSet";
            string outputDataFolder = "vectorizedData";

            _vectorizer = new Vectorizer();
            _vectorizer.Vectorizing(trainDataFolder, outputDataFolder);

            Console.WriteLine("Vectorizing done!");
        }
    }
}
