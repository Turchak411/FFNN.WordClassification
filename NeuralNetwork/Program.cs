using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using NeuralNetwork.ServicesManager;
using NeuralNetwork.ServicesManager.Vectors;

namespace NeuralNetwork
{
    class Program
    {
        private static NeuralNetwork _net;
        private static FileManager _fileManager;
        private static Vectorizer _vectorizer;

        private static object sync = new object();

        static void Main(string[] args)
        {
            #region Set process settings

            Process thisProc = Process.GetCurrentProcess();
            thisProc.PriorityClass = ProcessPriorityClass.High;

            #endregion

            _fileManager = new FileManager("memory.txt");

            // Initialize network:
            int numberOfOutputClasses = 13; // Количество наших классов
            int[] neuronByLayer = new[] { 45, numberOfOutputClasses };
            int receptors = 75;
            _net = new NeuralNetwork(neuronByLayer, receptors, _fileManager);

            // * Vectorizing words:
            //Vectorize();

            // Train network:
            TrainNet(receptors, numberOfOutputClasses);

            #region Testing

            double[] inputVector = _fileManager.ReadVector("inputDataTest1.txt");

            double[] outputVector = _net.Handle(inputVector);

            // Print result vector:
            Console.WriteLine("[Neuron] - [Activated value]");
            for (int i = 0; i < outputVector.Length; i++)
            {
                Console.WriteLine("{0} - {1:f3}", i, outputVector[i]);
            }
            #endregion

            Console.ReadKey();
        }

        private static void Vectorize()
        {
            string trainDataFolder = "data";
            string outputDataFolder = "vectorizedData";

            _vectorizer = new Vectorizer();
            _vectorizer.Vectorizing(trainDataFolder, outputDataFolder);
        }

        private static void TrainNet(int receptors, int numberOfOutputClasses)
        {
            #region Preparing learning DATA

            List<double[]> inputDataSets = new List<double[]>();
            List<double[]> outputDataSets = new List<double[]>();

            WordVectorLoader vectorLoader = new WordVectorLoader("vectorizedData");

            Console.WriteLine("Load input & output sets...");
            inputDataSets = vectorLoader.LoadVectorsData(receptors, numberOfOutputClasses, out outputDataSets);

            #endregion

            #region Net training

            Console.WriteLine("Training net...");

            double learningSpeed = 0;

            try
            {
                for (int i = 0; i < 15; i++) //15000; i++) //420000; i++)
                {
                    // Calculating learn-speed rate:
                    learningSpeed = 0.01 * Math.Pow(0.1, i / 150000);

                    for (int k = 0; k < inputDataSets.Count; k++)
                    {
                        _net.Handle(inputDataSets[k]);
                        _net.Teach(inputDataSets[k], outputDataSets[k], learningSpeed);
                    }
                }

                // Save network memory:
                _net.SaveMemory();

                Console.WriteLine("Training success!");
            }
            catch
            {
                Console.WriteLine("Training failed!");
            }

            #endregion
        }
    }
}
