using System.IO;
using System.Text;
using System.Collections.Generic;

namespace NeuralNetwork.ServicesManager
{
    public class WordVectorLoader
    {
        public List<double[]> LoadSecondaryVectorsData(string dataFolderPath, out List<double[]> outputDataSets)
        {
               outputDataSets = new List<double[]>();
            var inputDataSets = new List<double[]>();
            var dataSetsDirectories = Directory.GetDirectories(dataFolderPath);

            foreach (var directories in dataSetsDirectories)
            {
                var dataSetsFiles = Directory.GetFiles(directories);

                foreach (var file in dataSetsFiles)
                {
                    if (Path.GetFileName(file) == "inputSets.txt")
                    {
                        inputDataSets.AddRange(ReadDataSets(file));
                    }
                    else if (Path.GetFileName(file) == "outputSets.txt")
                    {
                        outputDataSets.AddRange(ReadDataSets(file));
                    }
                }
            }

            return inputDataSets;
        }

        private IEnumerable<double[]> ReadDataSets(string path)
        {
            var dataSetsList = new List<double[]>();
            using (var fileReader = new StreamReader(path, Encoding.UTF8))
            {
                while (!fileReader.EndOfStream)
                {
                    var str = fileReader.ReadLine()?.Split();
                    var dataSets = new double[str.Length-1];
                    for (int i = 0; i < str.Length-1; i++)
                    {
                        dataSets[i] = double.Parse(str[i]);
                    }

                    dataSetsList.Add(dataSets);
                }
            }

            return dataSetsList;
        }

        public List<double[]> LoadVectorsData(string dataFolderPath, out List<double[]> outputDataSets)
        {
            string[] trainFiles = Directory.GetFiles(dataFolderPath);

            // Input vector list:
            List<double[]> inputDataSets = new List<double[]>();
            // Output vector list:
            outputDataSets = new List<double[]>();

            for (int i = 0; i < trainFiles.Length; i++)
            {
                using (StreamReader fileReader = new StreamReader(trainFiles[i], Encoding.UTF8))
                {
                    double[] outputDataSet = new double[trainFiles.Length];
                    outputDataSet[i] = 1;

                    while (!fileReader.EndOfStream)
                    {
                        string[] readedWordData = fileReader.ReadLine().Split(' ');
                       
                        // Initial 'j' = 0 -> without string-word
                        if (readedWordData.Length == 77)
                        {
                            double[] inputDataSet = new double[readedWordData.Length - 2];
                            for (int j = 0; j < readedWordData.Length - 2; j++)
                            {
                                inputDataSet[j] = double.Parse(readedWordData[j + 1]);
                            }

                            inputDataSets.Add(inputDataSet);
                            outputDataSets.Add(outputDataSet);
                        }
                    }
                }
            }

            return inputDataSets;
        }

        #region Overload mehtod LoadSecondaryVectorsData

        public KeyValuePair<List<double[]>, List<double[]>> LoadSecondaryVectorsData(string dataFolderPath)
        {
            var inputDataSets = new List<double[]>();
            var outputDataSets = new List<double[]>();

            var dataSetsDirectories = Directory.GetDirectories(dataFolderPath);

            foreach (var directories in dataSetsDirectories)
            {
                var dataSetsFiles = Directory.GetFiles(directories);

                foreach (var file in dataSetsFiles)
                {
                    if (Path.GetFileName(file) == "inputSets.txt")
                    {
                        inputDataSets.AddRange(ReadDataSets(file));
                    }
                    else if (Path.GetFileName(file) == "outputSets.txt")
                    {
                        outputDataSets.AddRange(ReadDataSets(file));
                    }
                }
            }

            return new KeyValuePair<List<double[]>, List<double[]>>(inputDataSets, outputDataSets);
        }

        #endregion
    }
}
