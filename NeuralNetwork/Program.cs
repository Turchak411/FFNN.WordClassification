using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetwork
{
    class Program
    {
        private static NeuralNetwork _net;
        private static FileManager _fileManager;

        static void Main(string[] args)
        {
            _fileManager = new FileManager("memory.txt");
            int numberOfOutputClasses = 13; // Количество наших классов
            int[] neuronByLayer = new[] { 100, 65, numberOfOutputClasses };
            int receptors = 200;
            _net = new NeuralNetwork(neuronByLayer, receptors, _fileManager);

            // Train network:
            TrainNet(receptors, numberOfOutputClasses);
        }

        private static void TrainNet(int receptors, int numberOfOutputClasses)
        {
            #region Preparing learning DATA

            #region Preparing learning INPUT SETS

            List<double[]> inputDataSets = new List<double[]>();
            WordVectorLoader vectorLoader = new WordVectorLoader("data");

            inputDataSets = vectorLoader.LoadVectorsData(receptors);

            #endregion

            // Разделена загрузка входных и выходных сетов. Так затратнее по памяти, но правильно

            #region Preparing learning OUTPUT SETS (ANWSERS)

            List<double[]> outputDataSets = vectorLoader.LoadOutputSets(numberOfOutputClasses);

            #endregion

            #endregion

            #region Net training

            Console.WriteLine("Training net...");

            double learningSpeed = 0;

            try
            {
                for (int i = 0; i < 420000; i++)
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
