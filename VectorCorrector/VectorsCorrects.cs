using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VectorCorrector
{
    public class VectorsCorrects
    {
        public void CorrectVectors(ref List<double[]> vectors)
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


        public List<double[]> CorrectReductionVectors(List<double[]> vectors, int offsetStart, int offsetEnd)
        {
            var newVectors = new List<double[]>();
            vectors.ForEach(vector =>
            {
                var vec = new double[offsetEnd];
                for (int k = offsetStart, i = 0; k < offsetEnd; k++, i++)
                {
                    vec[i] = vector[k];
                }

                newVectors.Add(vec);
            });

            return newVectors;
        }

    }
}
