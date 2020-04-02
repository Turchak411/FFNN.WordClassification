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

        static void Main(string[] args)
        {
            int trainStartCount = 0;
            int trainEndCount = 10000;

            #region Set process settings

            Process thisProc = Process.GetCurrentProcess();
            thisProc.PriorityClass = ProcessPriorityClass.High;

            #endregion

            const int receptors = 75;

            const int numberOfOutputClasses = 1; // Количество наших классов
            int[] neuronByLayer = { 50, 50, numberOfOutputClasses };

            _fileManager = new FileManager();

            var networkTeacher = new NetworkTeacher(neuronByLayer, receptors, 46, _fileManager)
            {
                Iteration = trainEndCount,
                TestVectors = _fileManager.ReadVectors("inputDataTestPart_temp.txt")
            };

            networkTeacher.PreparingLearningData(false);

            if(networkTeacher.CheckMemory())
            { 
                networkTeacher.TrainNet(trainStartCount);

                networkTeacher.CommonTestColorized();

                networkTeacher.Visualize();

                networkTeacher.PrintLearnStatistic(true);

                if (networkTeacher.CheckMemory())
                {
                    networkTeacher.BackupMemory();
                }
            }
            else
            {
                Console.WriteLine("Train failed!");
            }

            Console.ReadKey();
        }
    }
}
