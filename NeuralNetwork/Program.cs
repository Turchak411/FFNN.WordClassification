using System;
using System.Collections.Generic;
using System.Diagnostics;
using NeuralNetwork.ServicesManager;
using NeuralNetwork.ServicesManager.Vectors;
using System.Text;

namespace NeuralNetwork
{
    class Program
    {
        private static NeuralNetwork _net;
        private static FileManager _fileManager;
        private static Vectorizer _vectorizer;
        private static Merger _merger;

        private static List<Coeficent> _coeficents;

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
            int[] neuronByLayer = new[] { 45,23, numberOfOutputClasses };
            int receptors = 75;
            _net = new NeuralNetwork(neuronByLayer, receptors, _fileManager);

            // * Vectorizing words:
            //Vectorize();

            // Train network:
            _coeficents = _fileManager.ReadVectors("inputDataTest.txt");
            TrainNet(receptors, numberOfOutputClasses);

            #region Testing

            // Test();

          

                
           
            #endregion

            Console.ReadKey();
        }

        private static void Test()
        {
            double[] inputVector = _fileManager.ReadVector("inputDataTest.txt");

            double[] outputVector = _net.Handle(inputVector);

            // Print result vector:
            Console.WriteLine("[Neuron] - [Activated value]");
            for (int i = 0; i < outputVector.Length; i++)
            {
                Console.WriteLine("{0} - {1:f3}", i, outputVector[i]);
            }
        }

        private static void TestResult(List<Coeficent> testVectors, int iterartion)
        {
            // Console.Clear();
            if (iterartion > 0) ClearLine(17);
            var result = new StringBuilder();
            result.Append($"\nИтерация обучения: {iterartion}\n");
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

        public static void ClearLine(int lines = 1)
        {
            for (int i = 1; i <= lines; i++)
            {
                Console.SetCursorPosition(0, Console.CursorTop - 1);
                Console.Write(new string(' ', Console.WindowWidth));
                Console.SetCursorPosition(0, Console.CursorTop - 1);
            }
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

            #region Vector merging

            _merger = new Merger();
            List<List<double[]>> list = _merger.MergeItems(inputDataSets, outputDataSets);

            inputDataSets = list[0];
            outputDataSets = list[1];

            #endregion

            #region Net training

            Console.WriteLine("Training net...");

            double learningSpeed = 0;

            try
            {
                using (var progress = new ProgressBar())
                {


                    for (int i = 0; i < 100; i++) //15000; i++) //420000; i++)
                    {
                        // Calculating learn-speed rate:
                        learningSpeed = 0.01 * Math.Pow(0.1, i / 150000);

                        for (int k = 0; k < inputDataSets.Count; k++)
                        {
                           
                            _net.Handle(inputDataSets[k]);
                            _net.Teach(inputDataSets[k], outputDataSets[k], learningSpeed);
                        }

                        progress.Report((double)i / 100);
                        TestResult(_coeficents, i);
                    }

                  

                    // Save network memory:
                    _net.SaveMemory();
                }

                Console.WriteLine("Training success!");
            }
            catch(Exception ex)
            {
                Console.WriteLine("Training failed!" + ex.Message);
            }

            #endregion
        }
    }
}
