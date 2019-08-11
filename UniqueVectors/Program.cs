using System;
using System.Collections.Generic;
using System.IO;
using UniqueVectors.ServicesManager;

namespace UniqueVectors
{
    static class Program
    {
        static void Main(string[] args)
        {
            //var fileManager = new FileManager();
            var searchUniqueVectors = new SearchUniqueVectors();

            var inputDataSets = LoadDataSet("inputSets.txt");
            var outputDataSets = LoadDataSet("outputSets.txt");

            var dataSets = searchUniqueVectors.CreateDataSets(inputDataSets, outputDataSets);
            var listDataSets = searchUniqueVectors.CheckUniqueVectors(dataSets, "inputSets.txt");

            List<float[]> inputSets = new List<float[]>();
            List<float[]> outputSets = new List<float[]>();

            foreach (var dataSet in listDataSets)
            {
                inputSets.Add(dataSet.Vectors);
                outputSets.Add(dataSet.Ideals);
            }

            SaveVectors(inputSets, "inputSetsNew.txt");
            SaveVectors(outputSets, "outputSetsNew.txt");

            Console.ReadKey();
        }

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
