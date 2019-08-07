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
        private static VectorsCorrects _vectorsCorrects;

        static void Main(string[] args)
        {
            var fileName = "auto & economics\\outputSets.txt";
            _fileManager = new FileManager(fileName);
            _vectorsCorrects = new VectorsCorrects();
            var vectors = _fileManager.Load();

            //_vectorsCorrects.CorrectVectors(ref vectors);
             var resultVectors = _vectorsCorrects.CorrectReductionVectors(vectors, 0, 2);
             
            _fileManager.Save(resultVectors);

            Console.WriteLine("Correcting vectors in {0} successfully!", fileName);
            Console.ReadKey();
        }
    }
}
