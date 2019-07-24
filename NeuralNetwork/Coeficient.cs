using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetwork
{
    public struct Coeficent
    {
        public string _word;
        public double[] _listFloat;

        public Coeficent(string word, double[] listFloat)
        {
            _word = word;
            _listFloat = listFloat;
        }
    }
}
