using System;
using System.Collections.Generic;
using System.IO;

namespace NeuralNetwork
{
    class Program
    {
        private static NeuralNetwork _net;
        private static FileManager _fileManager;
        private static Vectorizer _vectorizer;

        static void Main(string[] args)
        {
            _fileManager = new FileManager("memory.txt");
            int numberOfOutputClasses = 13; // Количество наших классов
            int[] neuronByLayer = new[] { 45, 23, numberOfOutputClasses };
            int receptors = 69;
            _net = new NeuralNetwork(neuronByLayer, receptors, _fileManager);

            // * Vectorizing words:
            Vectorize();

            // Train network:
            TrainNet(receptors, numberOfOutputClasses);

            #region Testing

            //double[] inputVector = new double[0];

            //using (StreamReader fileReader = new StreamReader("inputDataTest0.txt"))
            //{
            //    while (!fileReader.EndOfStream)
            //    {
            //        string[] readedData = fileReader.ReadLine().Split(' ');
            //        inputVector = new double[readedData.Length - 2];

            //        for (int i = 0; i < readedData.Length - 3; i++)
            //        {
            //            inputVector[i] = double.Parse(readedData[i + 1]);
            //        }
            //    }
            //}

            //double[] outputVector = _net.Handle(inputVector);

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

            #region Preparing learning INPUT SETS

            List<double[]> inputDataSets = new List<double[]>();
            WordVectorLoader vectorLoader = new WordVectorLoader("vectorizedData");

            Console.WriteLine("Load vectors data...");
            inputDataSets = vectorLoader.LoadVectorsData(receptors);

            #endregion

            // Разделена загрузка входных и выходных сетов. Так затратнее по памяти, но правильно

            #region Preparing learning OUTPUT SETS (ANWSERS)

            Console.WriteLine("Load output sets...");
            List<double[]> outputDataSets = vectorLoader.LoadOutputSets(numberOfOutputClasses);

            #endregion

            #endregion

            #region Net training

            Console.WriteLine("Training net...");

            double learningSpeed = 0;

            try
            {
                for (int i = 0; i < 15000; i++) //420000; i++)
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
