using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetwork
{
    public class WordVectorLoader
    {
        private string _dataFolderPath;

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

                using (StreamReader fileReader = new StreamReader(_dataFolderPath + "//" + trainFiles[i]))
                {
                    while (!fileReader.EndOfStream)
                    {
                        string[] readedWordData = fileReader.ReadLine().Split(' ');

                        // Initial 'j' = 0 -> without string-word
                        for (int j = 1; j < readedWordData.Length; j++)
                        {
                            inputDataSet[j] = double.Parse(readedWordData[j]);
                        }
                    }
                }

                inputDataSets.Add(inputDataSet);
            }

            return inputDataSets;
        }

        public List<double[]> LoadOutputSets(int numberOfOutputClasses)
        {
            string[] trainFiles = Directory.GetFiles(_dataFolderPath);

            List<double[]> outputDataSets = new List<double[]>();

            for (int i = 0; i < trainFiles.Length; i++)
            {
                using (StreamReader fileReader = new StreamReader(_dataFolderPath + "//" + trainFiles[i]))
                {
                    double[] outputDataSet = new double[numberOfOutputClasses];
                    outputDataSet[i] = 1;

                    for (int k = 0; k < System.IO.File.ReadAllLines(_dataFolderPath + "//" + trainFiles[i]).Length; k++)
                    {
                        outputDataSets.Add(outputDataSet);
                    }
                }
            }

            return outputDataSets;
        }
    }
}
