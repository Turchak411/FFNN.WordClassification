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

        public List<double[]> LoadVectorsData(int receptors)
        {
            string[] trainFiles = Directory.GetFiles(_dataFolderPath);

            List<double[]> inputDataSets = new List<double[]>();

            for (int i = 0; i < trainFiles.Length; i++)
            {
                double[] inputDataSet = new double[receptors];

                using (StreamReader fileReader = new StreamReader(trainFiles[i]))
                {
                    while (!fileReader.EndOfStream)
                    {
                        string[] readedWordData = fileReader.ReadLine().Split(' ');

                        // Initial 'j' = 0 -> without string-word
                        for (int j = 0; j < readedWordData.Length - 2; j++)
                        {
                            inputDataSet[j] = double.Parse(readedWordData[j + 1]);
                        }

                        inputDataSets.Add(inputDataSet);
                    }
                }
            }

            return inputDataSets;
        }

        public List<double[]> LoadOutputSets(int numberOfOutputClasses)
        {
            string[] trainFiles = Directory.GetFiles(_dataFolderPath);

            List<double[]> outputDataSets = new List<double[]>();

            for (int i = 0; i < trainFiles.Length; i++)
            {
                using (StreamReader fileReader = new StreamReader(trainFiles[i]))
                {
                    double[] outputDataSet = new double[numberOfOutputClasses];
                    outputDataSet[i] = 1;

                    Parallel.ForEach(File.ReadAllLines(trainFiles[i]),
                        item =>
                        {
                            lock (sync)
                            {
                                outputDataSets.Add(outputDataSet);
                            }
                        });
                    //for (int k = 0; k < File.ReadAllLines(trainFiles[i]).Length; k++)
                    //{
                    //    outputDataSets.Add(outputDataSet);
                    //}
                }
            }

            return outputDataSets;
        }
    }
}
