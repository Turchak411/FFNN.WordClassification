using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UniqueVectors.Core;

namespace UniqueVectors.ServicesManager
{
    public class SearchUniqueVectors
    {
        private static readonly object Sync = new object();

        public List<DataSets> CreateDataSets(List<float[]> inputDataSets, List<float[]> outputDataSets) =>
            inputDataSets.Select((t, k) => new DataSets(t, outputDataSets[k])).ToList();

        public List<DataSets> CheckUniqueVectors(List<DataSets> dataSets, string path)
        {
            var vocabulary = new Hash2VecTextReader().Read(path);
            var newDataSets = new List<DataSets>(dataSets);
            int count = 0;

            foreach (var representation in vocabulary.Words)
            {
                var distanceList = vocabulary.Distance(representation, 10, 2).ToList();
                var vectorsDuplicats = distanceList.AsParallel().Where(dis => dis.DistanceValue >= 0.9);

                // var distancesTo = vectorsDuplicats as DistanceTo[] ?? vectorsDuplicats.ToArray();
                var vectorsEnumerable = vectorsDuplicats.AsParallel().SelectMany(vec =>
                    //  newDataSets.Where(data => EqualsVectors(data.Vectors, vec.Representation.NumericVector)));
                    dataSets.Where(data => EqualsVectors(data.Vectors, vec.Representation.NumericVector))).ToList();

                var vector = newDataSets.AsParallel()
                    .FirstOrDefault(vec => EqualsVectors(vec.Vectors, representation.NumericVector));
                // var dataSetses = vectorsEnumerable as DataSets[] ?? vectorsEnumerable.ToArray();
                if (vector != null)
                {
                    newDataSets.Remove(vector);
                    //foreach (var vec in vectorsEnumerable)
                    Parallel.ForEach(vectorsEnumerable, vec =>
                    {
                        for (int i = 0; i < vector.Ideals.Length; i++)
                        {
                            if (vector.Ideals[i] < vec.Ideals[i])
                            {
                                vector.Ideals[i] = vec.Ideals[i];
                            }
                        }
                    });
                   
                    newDataSets.Add(vector);
                }

                //   var enumerable = vectorsEnumerable.Select(data => data.Select(vec => newDataSets.Remove(vec)));

                foreach (var data in vectorsEnumerable)
                {
                   newDataSets.Remove(data);
                }

                count++;
            }

            return newDataSets;
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
