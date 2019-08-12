using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Hash2Vec.ServiceManager.DistanceVectors;

namespace UniqueVectors.Core
{
    public static class Data
    {
        public static List<DataSets> NewDataSets  { get; set; } 
    }


    public class UniqueVectors
    {
        private readonly int _offsetStart;
        private readonly int _offsetEnd;

        private readonly Vocabulary _vocabulary;
        private readonly List<DataSets> _dataSets;

        private static object Sync = new object();

        public UniqueVectors(Vocabulary vocabulary, List<DataSets> dataSets, int offsetStart, int offsetEnd)
        {
            _vocabulary = vocabulary;
            _dataSets = dataSets;
            _offsetStart = offsetStart;
            _offsetEnd = offsetEnd;
            
        }


        public void GetUniqueVectors()
        {
            var count = 0;
            for (var k = _offsetStart; k < _offsetEnd; k++)
            {
                var distanceList = _vocabulary.Distance(_vocabulary.Words[k], 3, 2).ToList();
                var vectorsDuplicats = distanceList.AsParallel().Where(dis => dis.DistanceValue >= 0.95);

                var vectorsEnumerable = vectorsDuplicats.AsParallel().SelectMany(vec =>
                    _dataSets.Where(data => EqualsVectors(data.Vectors, vec.Representation.NumericVector))).ToList();

                var vector = _dataSets.AsParallel()
                    .FirstOrDefault(vec => EqualsVectors(vec.Vectors, _vocabulary.Words[k].NumericVector));
                if (vector != null)
                {
                    lock (Sync)
                    {
                        Data.NewDataSets.Remove(vector);
                        Parallel.ForEach(vectorsEnumerable, vec =>
                        {
                            for (var i = 0; i < vector.Ideals.Length; i++)
                                if (vector.Ideals[i] < vec.Ideals[i]) vector.Ideals[i] = vec.Ideals[i];
                        });

                        Data.NewDataSets.Add(vector);
                    }
                   
                }

                foreach (var data in vectorsEnumerable)
                {
                    lock (Sync)
                    {
                        Data.NewDataSets.Remove(data);
                    }
                }

                count++;
            }
        }

        private bool EqualsVectors(float[] vector1, float[] vector2)
        {
            var result = false;
            for (var i = 0; i < vector1.Length; i++)
            {
                if (vector1[i].Equals(vector2[i]))
                    result = true;
                else
                    return false;
            }

            return result;
        }

    }
}
