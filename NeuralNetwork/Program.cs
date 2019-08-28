using System;
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

            const int numberOfOutputClasses = 1; // Количество наших классов
            int[] neuronByLayer = { 50, 50, numberOfOutputClasses };

            _fileManager = new FileManager("memory.txt");

            var networkTeacher = new NetworkTeacher(neuronByLayer, receptors, 2, _fileManager)
            {
                Iteration = 4245,
                TestVectors = _fileManager.ReadVectors("inputDataTest6.txt")
            };

            //Vectorize();

            //networkTeacher.PreparingLearningData(true, true);

            networkTeacher.TrainNet(100);          

            Console.ReadKey();
        }

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
