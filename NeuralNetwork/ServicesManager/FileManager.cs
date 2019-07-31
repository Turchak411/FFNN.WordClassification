using System.Collections.Generic;
using System.IO;
using NeuralNetwork.ServicesManager.Vectors;

namespace NeuralNetwork.ServicesManager
{
    public class FileManager
    {
        private readonly string _dataPath;

        public FileManager() { }

        public FileManager(string dataPath) => _dataPath = dataPath;

        public double[] LoadMemory(int layerNumber, int neuronNumber)
        {
            double[] memory = new double[0];

            using (StreamReader fileReader = new StreamReader(_dataPath))
            {
                while (!fileReader.EndOfStream)
                {
                    string[] readedLine = fileReader.ReadLine().Split(' ');

                    if ((readedLine[0] == "layer_" + layerNumber) && (readedLine[1] == "neuron_" + neuronNumber))
                    {
                        memory = GetWeights(readedLine);
                    }
                }
            }

            return memory;
        }

        private double[] GetWeights(string[] readedLine)
        {
            double[] weights = new double[readedLine.Length - 2];

            for (int i = 0; i < weights.Length; i++)
            {
                weights[i] = double.Parse(readedLine[i + 2]);
            }

            return weights;
        }

        public void PrepareToSaveMemory()
        {
            File.Delete(_dataPath);
        }

        public void SaveMemory(int layerNumber, int neuronNumber, double[] weights)
        {
            using (StreamWriter fileWriter = new StreamWriter(_dataPath, true))
            {
                fileWriter.Write("layer_{0} neuron_{1}", layerNumber, neuronNumber);

                for (int i = 0; i < weights.Length; i++)
                {
                    fileWriter.Write(" " + weights[i]);
                }

                fileWriter.WriteLine("");
            }
        }

        public double[] ReadVector(string filePath)
        {
            double[] inputVector = new double[0];

            using (StreamReader fileReader = new StreamReader(filePath))
            {
                while (!fileReader.EndOfStream)
                {
                    string[] readedData = fileReader.ReadLine().Split(' ');
                    inputVector = new double[readedData.Length - 2];

                    for (int i = 0; i < readedData.Length - 2; i++)
                    {
                        inputVector[i] = double.Parse(readedData[i + 1]);
                    }
                }
            }

            return inputVector;
        }


        public List<Coeficent> ReadVectors(string filePath)
        {
            var vectors = new List<Coeficent>();
            using (StreamReader fileReader = new StreamReader(filePath))
            {
                while (!fileReader.EndOfStream)
                {
                    var readedData = fileReader.ReadLine().Split(' ');
                    var inputVector = new double[readedData.Length - 2];

                    for (int i = 0; i < readedData.Length - 2; i++)
                    {
                        inputVector[i] = double.Parse(readedData[i + 1]);
                    }

                    vectors.Add(new Coeficent(readedData[0],inputVector));
                }
            }

            return vectors;
        }

        /// <summary>
        /// Сохранение векторов из dataSets
        /// </summary>
        /// <param name="dataSets">dataSets</param>
        /// <param name="path">Путь к файлу</param>
        public void SaveVectors(List<double[]> dataSets, string path)
        {
            using (var sw = new StreamWriter(path))
            {
                dataSets.ForEach(dates =>
                {
                    foreach (var data in dates) sw.Write(data+ " ");
                    sw.WriteLine();
                });
            }
        }

        public List<double[]> LoadDataSet(string path)
        {
            List<double[]> sets = new List<double[]>();

            using (StreamReader fileReader = new StreamReader(path))
            {
                while (!fileReader.EndOfStream)
                {
                    string[] readedLine = fileReader.ReadLine().Split(' ');
                    double[] set = new double[readedLine.Length];

                    for (int i = 0; i < readedLine.Length; i++)
                    {
                        set[i] = double.Parse(readedLine[i]);
                    }

                    sets.Add(set);
                }
            }

            return sets;
        }
    }
}
