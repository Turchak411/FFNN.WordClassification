using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace NeuralNetwork.ServicesManager
{
    public class WordVectorLoader
    {
        private string _dataFolderPath;

        private static object sync = new object();

        private WordVectorLoader() { }

        public WordVectorLoader(string dataFolderPath)
        {
            _dataFolderPath = dataFolderPath;
        }

        public List<double[]> LoadVectorsData(int receptors, int numberOfOutputClasses, out List<double[]> outputDataSets)
        {
            string[] trainFiles = Directory.GetFiles(_dataFolderPath);

            // Input vector list:
            List<double[]> inputDataSets = new List<double[]>();
            // Output vector list:
            outputDataSets = new List<double[]>();

            for (int i = 0; i < trainFiles.Length; i++)
            {
                using (StreamReader fileReader = new StreamReader(trainFiles[i]))
                {
                    double[] outputDataSet = new double[numberOfOutputClasses];
                    outputDataSet[i] = 1;

                    while (!fileReader.EndOfStream)
                    {
                        double[] inputDataSet = new double[receptors];
                        string[] readedWordData = fileReader.ReadLine().Split(' ');

                        // Initial 'j' = 0 -> without string-word
                        for (int j = 0; j < readedWordData.Length - 2; j++)
                        {
                            inputDataSet[j] = double.Parse(readedWordData[j + 1]);
                        }

                        inputDataSets.Add(inputDataSet);
                        outputDataSets.Add(outputDataSet);
                    }
                }
            }

            return inputDataSets;
        }
    }
}
