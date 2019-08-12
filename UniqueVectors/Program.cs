using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using UniqueVectors.ServicesManager;

namespace UniqueVectors
{
    static class Program
    {
        static void Main(string[] args)
        {
            //var fileManager = new FileManager();
            Console.WriteLine("\n>>> Start Processing >>>");
            var stopWatch = Stopwatch.StartNew();

            var searchUniqueVectors = new SearchUniqueVectors();

            Console.WriteLine("\n>>> Read Data Sets >>>");
            ShowTime(stopWatch.Elapsed);

            var inputDataSets = LoadDataSet("inputSets.txt");
            var outputDataSets = LoadDataSet("outputSets.txt");

            Console.WriteLine("\n>>> Start Processing UniqueVectors >>>");
            ShowTime(stopWatch.Elapsed);

            var dataSets = searchUniqueVectors.CreateDataSets(inputDataSets, outputDataSets);
            var listDataSets = searchUniqueVectors.CheckUniqueVectorsParallel(dataSets, "inputSets.txt",2);

            var inputSets = new List<float[]>();
            var outputSets = new List<float[]>();

            foreach (var dataSet in listDataSets)
            {
                inputSets.Add(dataSet.Vectors);
                outputSets.Add(dataSet.Ideals);
            }

            Console.WriteLine("\n>>> Save UniqueVectors >>>\n");
            ShowTime(stopWatch.Elapsed);

            SaveVectors(inputSets, "inputSetsNew.txt");
            SaveVectors(outputSets, "outputSetsNew.txt");

            ShowTime(stopWatch.Elapsed);
            Console.WriteLine("\n<<< Stop Processing <<<");
            Console.ReadKey();
        }

        private static void ShowTime(TimeSpan time) =>
            Console.WriteLine($">>> Time interval: {time.Hours:00}:{time.Minutes:00}:{time.Seconds:00}.{time.Milliseconds / 10:00} >>>");

        /// <summary>
        /// Сохранение векторов из dataSets
        /// </summary>
        /// <param name="dataSets">dataSets</param>
        /// <param name="path">Путь к файлу</param>
        public static void SaveVectors(List<float[]> dataSets, string path)
        {
            using (var sw = new StreamWriter(path))
            {
                dataSets.ForEach(dates =>
                {
                    foreach (var data in dates) sw.Write(data + " ");
                    sw.WriteLine();
                });
            }
        }


        public static List<float[]> LoadDataSet(string path)
        {
            List<float[]> sets = new List<float[]>();

            using (StreamReader fileReader = new StreamReader(path))
            {
                while (!fileReader.EndOfStream)
                {
                    string[] readedLine = fileReader.ReadLine().Split(' ');
                    float[] set = new float[readedLine.Length - 1];

                    for (int i = 0; i < readedLine.Length - 1; i++)
                    {
                        set[i] = float.Parse(readedLine[i]);
                    }

                    sets.Add(set);

                }
            }

            return sets;
        }

    }
}
