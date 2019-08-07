using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VectorCorrector
{
    class Program
    {
        private static FileManager _fileManager;

        static void Main(string[] args)
        {
            var fileName = "outputSets.txt";
            _fileManager = new FileManager(fileName);

            var vectors = _fileManager.Load();

            CorrectVectors(vectors);

            _fileManager.Save(vectors);

            Console.WriteLine("Correcting vectors in {0} successfully!", fileName);
            Console.ReadKey();
        }

        private static void CorrectVectors(List<double[]> vectors)
        {
            foreach (var vector in vectors)
            {
                for (int i = 0; i < vector.Length; i++)
                {
                    if (vector[i] > 1)
                    {
                        vector[i] = 1;
                    }
                }
            }
        }
    }
}
