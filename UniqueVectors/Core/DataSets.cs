using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniqueVectors.Core
{
    public class DataSets
    {
        public float[] Vectors { get; }

        public float[] Ideals { get; }

        public DataSets(float[] vectors, float[] ideals)
        {
            Vectors = vectors;
            Ideals = ideals;
        }
    }
}
