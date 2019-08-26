using System;
using System.Text;
using System.Collections.Generic;
using System.Diagnostics;
using NeuralNetwork.ServicesManager;
using NeuralNetwork.ServicesManager.Vectors;

namespace NeuralNetwork.Core
{
    public class NetworkTeacher
    {
        private Merger _merger;
        private FileManager _fileManager;

        private NeuralNetwork _net;

        public int Iteration { get; set; } = 20;

        public List<Coeficent> TestVectors { get; set; }

        public NetworkTeacher(NeuralNetwork net, FileManager fileManager)
        {
            _net = net;
            _fileManager = fileManager;
        }

        public void TestResult(List<Coeficent> testVectors, int outputSetLength, int iteration)
        {
            if (testVectors == null) return;

            if (iteration > 0) ClearLine(outputSetLength+4);
            var result = new StringBuilder();
            result.Append($"\nИтерация обучения: {iteration}\n");
            testVectors.ForEach(vector => result.Append($"   {vector._word}     "));
            result.Append('\n');
            for (int k = 0; k < outputSetLength; k++)
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

        public void TestResult(List<Coeficent> testVectors, int outputSetLength)
        {
            if (testVectors == null) return;

            var result = new StringBuilder();
            testVectors.ForEach(vector => result.Append($"   {vector._word}     "));
            result.Append('\n');
            for (int k = 0; k < outputSetLength; k++)
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

        public void PreparingLearningData(bool primaryLoad = true, bool withMerging = false)
        {
            #region Load data from files
            var stopWatch = Stopwatch.StartNew();
            var vectorLoader = new WordVectorLoader();
            Console.WriteLine("Load input & output sets...");

            var inputDataSets = primaryLoad
                ? vectorLoader.LoadVectorsData("vectorizedData", out var outputDataSets)
                : vectorLoader.LoadSecondaryVectorsData("data", out  outputDataSets);

            ShowTime(stopWatch.Elapsed);
            #endregion

            #region Vector merging

            if (withMerging)
            {
                Console.WriteLine("Start vector merging...");
                _merger = new Merger();
                var list = _merger.MergeItems(inputDataSets, outputDataSets);
                Console.WriteLine("Save results...");

                _fileManager.SaveVectors(list[0], "inputSets.txt");
                _fileManager.SaveVectors(list[1], "outputSets.txt");
            }
            else
            {
                Console.WriteLine("Save results...");

                _fileManager.SaveVectors(inputDataSets, "inputSets.txt");
                _fileManager.SaveVectors(outputDataSets, "outputSets.txt");
            }

            ShowTime(stopWatch.Elapsed); stopWatch.Stop();
            Console.WriteLine("Save result done!");
            #endregion
        }

        private static void ShowTime(TimeSpan time) =>
            Console.WriteLine($">>> Time interval: {time.Hours:00}:{time.Minutes:00}:{time.Seconds:00}.{time.Milliseconds / 10:00} >>>");

        /// <summary>
        /// Обучение нейросети
        /// </summary>
        public void TrainNet(bool withSort = false)
        {
            #region Load data from file

            List<double[]> inputDataSets = _fileManager.LoadDataSet("inputSets.txt");
            List<double[]> outputDataSets = _fileManager.LoadDataSet("outputSets.txt");

            #region Sorting

            if (withSort)
            {
                Console.WriteLine("Sort starting...");

                Random rnd = new Random();

                for (int i = 0; i < inputDataSets.Count; i++)
                {
                    double[] tempInput = inputDataSets[0];
                    double[] tempOutput = outputDataSets[0];
                    inputDataSets.RemoveAt(0);
                    outputDataSets.RemoveAt(0);

                    int rndIndex = rnd.Next(inputDataSets.Count);

                    inputDataSets.Insert(rndIndex, tempInput);
                    outputDataSets.Insert(rndIndex, tempOutput);
                }

                Console.WriteLine("Sorted sets saving...");
                _fileManager.SaveVectors(inputDataSets, "inputSet_sorted.txt");
                _fileManager.SaveVectors(outputDataSets, "outputSet_sorted.txt");
            }

            #endregion

            #endregion

            int k = 0;
            Console.WriteLine("Training net...");
            try
            {
                using (var progress = new ProgressBar())
                {
                    for (int iteration = 0; iteration < Iteration; iteration++)
                    {
                        // Calculating learn-speed rate:
                        var learningSpeed = 0.01 * Math.Pow(0.1, iteration / 150000);
                        using (var progress1 = new ProgressBar())
                        {
                            for (k = 40001; k < inputDataSets.Count; k++)
                            {
                                _net.Handle(inputDataSets[k]);
                                _net.Teach(inputDataSets[k], outputDataSets[k], learningSpeed);
                                progress1.Report((double)k / inputDataSets.Count);
                            }   
                        }

                        progress.Report((double) iteration / Iteration);
                        TestResult(TestVectors, outputDataSets[0].Length, iteration);
                    }

                    // Save network memory:
                    _net.SaveMemory();
                }

                Console.WriteLine("Training success!");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Training failed! " + ex.Message + Convert.ToString(k));
            }
        }

        public void DoTest()
        {
            #region Load data from file

            List<double[]> inputDataSets = _fileManager.LoadDataSet("inputSets.txt");
            List<double[]> outputDataSets = _fileManager.LoadDataSet("outputSets.txt");

            #endregion

            TestResult(TestVectors, outputDataSets[0].Length);
        }
    }
}
