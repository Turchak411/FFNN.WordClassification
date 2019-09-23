using System;
using System.Diagnostics;
using NeuralNetwork.Core;
using NeuralNetwork.ServicesManager;
using NeuralNetwork.ServicesManager.Vectors;

namespace NeuralNetwork
{
    static class Program
    {
        private static FileManager _fileManager;

        private static Vectorizer _vectorizer;

        static void Main(string[] args)
        {
            int trainStartCount = 1000;
            int trainEndCount = 1200;

            #region Set process settings

            Process thisProc = Process.GetCurrentProcess();
            thisProc.PriorityClass = ProcessPriorityClass.High;

            #endregion

            const int receptors = 75;

            const int numberOfOutputClasses = 1; // Количество наших классов
            int[] neuronByLayer = { 50, 50, numberOfOutputClasses };

            _fileManager = new FileManager();

            var networkTeacher = new NetworkTeacher(neuronByLayer, receptors, 13, _fileManager)
            {
                Iteration = trainEndCount,
                TestVectors = _fileManager.ReadVectors("inputDataTestPart1.txt")
            };

            //networkTeacher.PreparingLearningData(true);

            if(networkTeacher.CheckMemory())
            {
                networkTeacher.TrainNet(trainStartCount);

                networkTeacher.CommonTestColorized();

                networkTeacher.Visualize();

                networkTeacher.PrintLearnStatistic();

                if (networkTeacher.CheckMemory())
                {
                    networkTeacher.BackupMemory(trainEndCount);
                }
            }
            else
            {
                Console.WriteLine("Train failed! Invalid memory!");
            }

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
