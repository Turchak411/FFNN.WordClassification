using System.Collections.Generic;
using System.IO;
using System.Text;

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

        public List<double[]> LoadVectorsData(out List<double[]> outputDataSets)
        {
            string[] trainFiles = Directory.GetFiles(_dataFolderPath);

            // Input vector list:
            List<double[]> inputDataSets = new List<double[]>();
            // Output vector list:
            outputDataSets = new List<double[]>();

            for (int i = 0; i < trainFiles.Length; i++)
            {
                using (StreamReader fileReader = new StreamReader(trainFiles[i], Encoding.Default))
                {
                    double[] outputDataSet = new double[trainFiles.Length];
                    outputDataSet[i] = 1;

                    while (!fileReader.EndOfStream)
                    {
                        string[] readedWordData = fileReader.ReadLine().Split(' ');
                        double[] inputDataSet = new double[readedWordData.Length - 2];

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
