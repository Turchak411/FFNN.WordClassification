using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VectorCorrector
{
    public class FileManager
    {
        private string _filePath;

        public FileManager(string filePath = "outputSets.txt") => _filePath = filePath;

        public List<double[]> Load()
        {
            List<double[]> vectors = new List<double[]>();

            using (var fileReader = new StreamReader(_filePath))
            {
                while (!fileReader.EndOfStream)
                {
                    string[] readedVectorText = fileReader.ReadLine().Split(' ');
                    double[] vector = new Double[readedVectorText.Length - 1];

                    for (int i = 0; i < vector.Length; i++)
                    {
                        vector[i] = double.Parse(readedVectorText[i]);
                    }

                    vectors.Add(vector);
                }
            }

            return vectors;
        }

        public void Save(List<double[]> vectors)
        {
            using (var fileWriter = new StreamWriter(_filePath))
            {
                foreach (var vector in vectors)
                {
                    for (int i = 0; i < vector.Length; i++)
                    {
                        fileWriter.WriteLine(vector[i] + " ");
                    }
                }
            }
        }
    }
}
