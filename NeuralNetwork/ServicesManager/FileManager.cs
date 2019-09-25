using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using NeuralNetwork.ServicesManager.Vectors;

namespace NeuralNetwork.ServicesManager
{
    public class FileManager
    {
        private readonly string _dataPath;

        private FileManager() { }

        public FileManager(string dataPath = "memory.txt")
        {
            // Запуск процесса генерации памяти в случае ее отсутствия:
            if (File.Exists(dataPath))
            {
                _dataPath = dataPath;
            }
            else
            {
                Console.WriteLine("NeuralNet memory is missing! Start generating process...");
                _dataPath = dataPath;
                WeightsGenerator.Program.Main(null);
            }
        }

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

        public double[] LoadMemory(int layerNumber, int neuronNumber, string memoryPath)
        {
            double[] memory = new double[0];

            if (!File.Exists(memoryPath))
            {
                // Создание памяти для отдельного класса в случае отсутствия таковой
                File.Copy(_dataPath, memoryPath);
            }

            using (StreamReader fileReader = new StreamReader(memoryPath))
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

        public void PrepareToSaveMemory(string path)
        {
            File.Delete(path);
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

        public void SaveMemory(int layerNumber, int neuronNumber, double[] weights, string path)
        {
            using (StreamWriter fileWriter = new StreamWriter(path, true))
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
            if (!File.Exists(filePath)) return null;

            var vectors = new List<Coeficent>();
            using (StreamReader fileReader = new StreamReader(filePath, Encoding.Default))
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
                    double[] set = new double[readedLine.Length - 1];
                   
                        for (int i = 0; i < readedLine.Length - 1; i++)
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
