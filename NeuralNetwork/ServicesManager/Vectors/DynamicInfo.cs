using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetwork.ServicesManager.Vectors
{
    public struct DynamicInfo
    {
        public char _lastSymbol;
        public double _lastAnwser;

        public DynamicInfo(char lastSymbol, double lastAnwser)
        {
            _lastSymbol = lastSymbol;
            _lastAnwser = lastAnwser;
        }
    }
}
