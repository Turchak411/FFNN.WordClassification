using System;
using System.Text;
using System.IO;
using System.Collections.Generic;
using System.Diagnostics;
using NeuralNetwork.ServicesManager;
using NeuralNetwork.ServicesManager.Vectors;
using NeuralNetwork.ServicesManager.Visualize;

namespace NeuralNetwork.Core
{
    public class NetworkTeacher
    {
        private List<NeuralNetwork> _netsList;

        private Merger _merger;
        private FileManager _fileManager;
        private MemoryChecker _memoryChecker;

        private TrainVisualizator _trainVisualizator;

        private List<List<DynamicInfo>> _anwserDynamicInfos;

        public int Iteration { get; set; } = 20;

        public List<Coeficent> TestVectors { get; set; }

        public NetworkTeacher(int[] neuronByLayer, int receptors, int netsCount, FileManager fileManager)
        {
            _netsList = new List<NeuralNetwork>();

            // Ицициализация сети по одинаковому шаблону:
            for(int i = 0; i < netsCount; i++)
            {
                _netsList.Add(new NeuralNetwork(neuronByLayer, receptors, fileManager, "memory_" + i.ToString() + ".txt"));
            }

            _fileManager = fileManager;

            // Инициализация последних ответов для учета динамики обучения:
            _anwserDynamicInfos = new List<List<DynamicInfo>>();
            for(int i = 0; i < netsCount; i++)
            {
                _anwserDynamicInfos.Add(new List<DynamicInfo>());
            }
        }

        private void TestResult(int outputSetLength, int iteration, int startIteration)
        {
            if (TestVectors == null) return;

            if (iteration > startIteration) ClearLine(outputSetLength+4);
            var result = new StringBuilder();
            result.Append($"\nИтерация обучения: {iteration}\n");
            TestVectors.ForEach(vector => result.Append($"   {vector._word}     "));
            result.Append('\n');

            for (int i = 0; i < _netsList.Count; i++)
            {
                for(int k = 0; k < TestVectors.Count; k++)
                {
                    // Получение ответа:
                    var outputVector = _netsList[i].Handle(TestVectors[k]._listFloat);

                    // Запись знака динамики для следующего ответа:
                    if(_anwserDynamicInfos[i].Count == TestVectors.Count)
                    {
                        _anwserDynamicInfos[i][k] = new DynamicInfo(
                                                        outputVector[0] > _anwserDynamicInfos[i][k]._lastAnwser ? '+' : '-',
                                                        outputVector[0]
                                                        );
                    }
                    else
                    {
                        _anwserDynamicInfos[i].Add(new DynamicInfo('=', outputVector[0]));
                    }

                    result.Append($"{outputVector[0]:f5} ({_anwserDynamicInfos[i][k]._lastSymbol})\t");
                }
                result.Append('\n');
            }

            Console.WriteLine(result);
        }

        private void TestResultExtended(int outputSetLength, int iteration, int startIteration)
        {
            if (TestVectors == null) return;

            if (iteration > startIteration) ClearLine(outputSetLength + 4);
            var result = new StringBuilder();
            result.Append($"\nИтерация обучения: {iteration}\n");
            TestVectors.ForEach(vector => result.Append($"   {vector._word}     "));
            result.Append('\n');

            for (int i = 0; i < _netsList.Count; i++)
            {
                for (int k = 0; k < TestVectors.Count; k++)
                {
                    // Получение ответа:
                    var outputVector = _netsList[i].Handle(TestVectors[k]._listFloat);

                    double different = 0;

                    // Запись знака динамики для следующего ответа:
                    if (_anwserDynamicInfos[i].Count == TestVectors.Count)
                    {
                        different = Math.Abs(outputVector[0] - _anwserDynamicInfos[i][k]._lastAnwser);

                        _anwserDynamicInfos[i][k] = new DynamicInfo(
                            outputVector[0] > _anwserDynamicInfos[i][k]._lastAnwser ? '+' : '-',
                            outputVector[0]
                        );
                    }
                    else
                    {
                        _anwserDynamicInfos[i].Add(new DynamicInfo('=', outputVector[0]));
                    }

                    result.Append($"{outputVector[0]:f5} ({_anwserDynamicInfos[i][k]._lastSymbol}) ({_anwserDynamicInfos[i][k]._lastSymbol}{different:f5})\t");
                }
                result.Append('\n');
            }

            Console.WriteLine(result);
        }

        public void CommonTest()
        {
            if (TestVectors == null) return;

            var result = new StringBuilder();
            TestVectors.ForEach(vector => result.Append($"   {vector._word}     "));
            result.Append('\n');

            for (int i = 0; i < _netsList.Count; i++)
            {
                for (int k = 0; k < TestVectors.Count; k++)
                {
                    // Получение ответа:
                    var outputVector = _netsList[i].Handle(TestVectors[k]._listFloat);

                    result.Append($"{outputVector[0]:f5}\t");
                }
                result.Append('\n');
            }

            Console.WriteLine(result);
        }

        public void CommonTestColorized()
        {
            if (TestVectors == null) return;

            var result = new StringBuilder();
            TestVectors.ForEach(vector => result.Append($"   {vector._word}     "));
            result.Append('\n');

            for (int i = 0; i < _netsList.Count; i++)
            {
                for (int k = 0; k < TestVectors.Count; k++)
                {
                    // Получение ответа:
                    var outputVector = _netsList[i].Handle(TestVectors[k]._listFloat);

                    // Костыль: для корректного теста сетям нужна по крайней мере одна итерация обучения:
                    _netsList[i].Teach(TestVectors[k]._listFloat, new double[1] {1}, 0.01); //0.000000000000001);

                    //result.Append($"{outputVector[0]:f5}\t");

                    Console.ForegroundColor = GetColorByActivation(outputVector[0]);
                    Console.Write($"{outputVector[0]:f5}\t");
                }

                Console.ForegroundColor = ConsoleColor.Gray;
                Console.Write('\n');
            }

            Console.ForegroundColor = ConsoleColor.Gray;
        }

        private ConsoleColor GetColorByActivation(double value)
        {
            if(value > 0.95)
            {
                return ConsoleColor.Red;
            }

            if (value > 0.8)
            {
                return ConsoleColor.Magenta;
            }

            if (value > 0.5)
            {
                return ConsoleColor.Yellow;
            }

            return ConsoleColor.Gray;
        }

        public void Visualize(string path = "trainVisualizationData.txt")
        {
            // Create visualizator object and load prevData:
            _trainVisualizator = new TrainVisualizator();

            // Save current data:
            using (StreamWriter fileWriter = new StreamWriter(path))
            {
                for (int i = 0; i < TestVectors.Count; i++)
                {
                    fileWriter.Write(TestVectors[i]._word);

                    for (int k = 0; k < _netsList.Count; k++)
                    {
                        fileWriter.Write(" " + _netsList[k].Handle(TestVectors[i]._listFloat)[0]);
                    }

                    fileWriter.WriteLine();
                }
            }

            _trainVisualizator.DrawTestVectorsGraphics();
        }

        private void Logging(int testPassed, int testFailed, int testFailedLowActivationCause, string logsDirectoryName = ".logs")
        {
            // Check for existing main logs-directory:
            if (!Directory.Exists(logsDirectoryName))
            {
                Directory.CreateDirectory(logsDirectoryName);
            }

            // Save logs:
            using (StreamWriter fileWriter = new StreamWriter(logsDirectoryName + "/" + Iteration + ".txt"))
            {
                fileWriter.WriteLine("Test passed: " + testPassed);
                fileWriter.WriteLine("Test failed: " + testFailed);
                fileWriter.WriteLine("     - Low activation causes: " + testFailedLowActivationCause);
                fileWriter.WriteLine("Percent learned: {0:f2}", (double)testPassed * 100 / (testPassed + testFailed));
            }
            
            Console.WriteLine("Learn statistic logs saved in .logs!");
        }

        public void PrintLearnStatistic(bool withLogging = false)
        {
            Console.WriteLine("Start calculating statistic...");

            int testPassed = 0;
            int testFailed = 0;
            int testFailed_lowActivationCause = 0;

            #region Load data from file

            List<double[]> inputDataSets = _fileManager.LoadDataSet("inputSets.txt");
            List<double[]> outputDataSets = _fileManager.LoadDataSet("outputSets.txt");

            #endregion

            for (int i = 0; i < inputDataSets.Count; i++)
            {
                List<double> netResults = new List<double>();

                for (int k = 0; k < _netsList.Count; k++)
                {
                    // Получение ответа:
                    netResults.Add(_netsList[k].Handle(inputDataSets[i])[0]);
                }

                // Поиск максимально активирующейся сети (класса) с заданным порогом активации:
                int maxIndex = FindMaxIndex(netResults, 0.8);

                if (maxIndex == -1)
                {
                    testFailed++;
                    testFailed_lowActivationCause++;
                }
                else
                {
                    if (outputDataSets[i][maxIndex] != 1)
                    {
                        testFailed++;
                    }
                    else
                    {
                        testPassed++;
                    }
                }
            }

            // Logging (optional):
            if (withLogging)
            {
                Logging(testPassed, testFailed, testFailed_lowActivationCause);
            }

            Console.WriteLine("Test passed: {0}\nTest failed: {1}\n     - Low activation causes: {2}\nPercent learned: {3:f2}", testPassed,
                                                                                                                           testFailed,
                                                                                                                           testFailed_lowActivationCause,
                                                                                                                           (double)testPassed * 100 / (testPassed + testFailed));
        }

        public bool CheckMemory()
        {
            bool isValid = true;

            Console.WriteLine("Start memory cheking...");

            _memoryChecker = new MemoryChecker();

            for(int i = 0; i < _netsList.Count; i++)
            {
                if(_memoryChecker.IsValid("memory_" + i + ".txt"))
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("memory_" + i + " - is valid.");
                }
                else
                {
                    isValid = false;
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("memory_" + i + " - is invalid!");
                }
            }

            Console.ForegroundColor = ConsoleColor.Gray;

            return isValid;
        }

        private int FindMaxIndex(List<double> netResults, double threshold = 0.8)
        {
            int maxIndex = -1;
            double maxValue = -1;

            for(int i = 0; i < netResults.Count; i++)
            {
                if(maxValue < netResults[i] && netResults[i] >= threshold)
                {
                    maxIndex = i;
                    maxValue = netResults[i];
                }
            }

            return maxIndex;
        }

        public void BackupMemory(string backupsDirectoryName = ".memory_backups")
        {
            // Check for existing main backups-directory:
            if(!Directory.Exists(backupsDirectoryName))
            {
                Directory.CreateDirectory(backupsDirectoryName);
            }

            // Check for already-existing sub-directory (trainCount-named):
            if (!Directory.Exists(backupsDirectoryName + "/ " + Iteration))
            {
                Directory.CreateDirectory(backupsDirectoryName  + "/" + Iteration);
            }

            // Saving memory:
            for (int i = 0; i < _netsList.Count; i++)
            {
                _netsList[i].SaveMemory(backupsDirectoryName + "/" + Iteration + "/memory_" + i + ".txt");
            }

            Console.WriteLine("Memory backuped!");
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
        /// Обучение сети
        /// </summary>
        /// <param name="startIteration"></param>
        /// <param name="withSort"></param>
        public void TrainNet(int startIteration = 0, bool withSort = false)
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
                    for (int iteration = startIteration; iteration < Iteration; iteration++)
                    {
                        // Calculating learn-speed rate:
                        var learningSpeed = 0.01 * Math.Pow(0.1, iteration / 150000);
                        using (var progress1 = new ProgressBar())
                        {
                            for (k = 0; k < inputDataSets.Count; k++)
                            {
                                for(int j = 0; j < outputDataSets[k].Length; j++)
                                {
                                    _netsList[j].Handle(inputDataSets[k]);

                                    // Передает для обучения только 1 элемент выходного вектора
                                    // (Класс на который конкретной сети нужно активироваться)
                                    double[] outputDataSetArray = new double[1] { outputDataSets[k][j] };

                                    _netsList[j].Teach(inputDataSets[k], outputDataSetArray, learningSpeed);
                                }

                                progress1.Report((double) k / inputDataSets.Count);
                            }   
                        }

                        progress.Report((double) iteration / Iteration);
                        TestResultExtended(outputDataSets[0].Length, iteration, startIteration);
                    }

                    // Save network memory:
                    for(int i = 0; i < _netsList.Count; i++)
                    {
                        _netsList[i].SaveMemory("memory_" + i.ToString() + ".txt");
                    }

                    // Костыль связанный с подходом в виде ансамбля нейросетей 
                    // на сохранение памяти (чтобы запускалось в следующий раз)
                    _netsList[0].SaveMemory("memory_0.txt");
                }

                Console.WriteLine("Training success!");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Training failed! " + ex.Message + Convert.ToString(k));
            }
        }
    }
}
