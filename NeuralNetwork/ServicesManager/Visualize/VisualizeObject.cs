using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetwork.ServicesManager.Visualize
{
    public struct VisualizeObject
    {
        public string _word;
        public List<double> _points;

        public VisualizeObject(string word, List<double> points)
        {
            _word = word;
            _points = points;
        }
    }
}
