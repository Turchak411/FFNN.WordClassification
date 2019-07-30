using System;
using System.Text;
using System.Collections.Generic;
using NeuralNetwork.ServicesManager;
using NeuralNetwork.ServicesManager.Vectors;

namespace NeuralNetwork.Core
{
    public class TeachNetwork
    {
        private readonly NeuralNetwork _net;

        private readonly int _receptors;
        private readonly int _numberOfOutputClasses;

        public int Iteration { get; set; } = 20;

        public List<Coeficent> TestVectors { get; set; }

      

        public TeachNetwork(int receptors, int numberOfOutputClasses, int[] neuronByLayer, string memoryPath)
        {
            _receptors = receptors;
            _numberOfOutputClasses = numberOfOutputClasses;
            _net = new NeuralNetwork(neuronByLayer, receptors, new FileManager(memoryPath));
        }

        /// <summary>
        /// Test neural network
        /// </summary>
        /// <param name="inputVector">Вектор для тестирования</param>
        public void Test(double[] inputVector)
        {
            var outputVector = _net.Handle(inputVector);

            // Print result vector:
            Console.WriteLine("[Neuron] - [Activated value]");

            for (int i = 0; i < outputVector.Length; i++)
            {
                Console.WriteLine("{0} - {1:f3}", i, outputVector[i]);
            }
        }

        public void TestResult(List<Coeficent> testVectors, int iteration)
        {
            if (iteration > 0) ClearLine(17);
            var result = new StringBuilder();
            result.Append($"\nИтерация обучения: {iteration}\n");
            testVectors.ForEach(vector => result.Append($"   {vector._word}     "));
            result.Append('\n');
            for (int k = 0; k < 13; k++)
            {
                foreach (var vector in testVectors)
                {
                    var outputVector = _net.Handle(vector._listFloat);
                    result.Append($"{k} - {outputVector[k]:f3}\t");
                }
                result.Append('\n');
            }

            Console.WriteLine(result);
        }

        /// <summary>
        /// Очистка строк в консоли
        /// </summary>
        /// <param name="lines"></param>
        private void ClearLine(int lines = 1)
        {
            for (int i = 1; i <= lines; i++)
            {
                Console.SetCursorPosition(0, Console.CursorTop - 1);
                Console.Write(new string(' ', Console.WindowWidth));
                Console.SetCursorPosition(0, Console.CursorTop - 1);
            }
        }

        /// <summary>
        /// Обучение нейросети
        /// </summary>
        public void TrainNet()
        {
            #region Preparing learning DATA

            var vectorLoader = new WordVectorLoader("vectorizedData");
            Console.WriteLine("Load input & output sets...");
            var inputDataSets = vectorLoader.LoadVectorsData(_receptors, _numberOfOutputClasses, out var outputDataSets);

            #endregion

            #region Net training

            Console.WriteLine("Training net...");
            try
            {
                using (var progress = new ProgressBar())
                {
                    for (int iteration = 0; iteration < Iteration; iteration++)
                    {
                        // Calculating learn-speed rate:
                        var learningSpeed = 0.01 * Math.Pow(0.1, iteration / 150000);
                        for (int k = 0; k < inputDataSets.Count; k++)
                        {
                            _net.Handle(inputDataSets[k]);
                            _net.Teach(inputDataSets[k], outputDataSets[k], learningSpeed);
                        }

                        progress.Report((double) iteration / 100);
                        TestResult(TestVectors, iteration);
                    }

                    // Save network memory:
                    _net.SaveMemory();
                }

                Console.WriteLine("Training success!");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Training failed!" + ex.Message);
            }

            #endregion
        }
    }
}
