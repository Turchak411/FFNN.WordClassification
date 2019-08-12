using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Hash2Vec.ServiceManager.ReadingVectors;
using NeuralNetwork;
using UniqueVectors.Core;

namespace UniqueVectors.ServicesManager
{
    public class SearchUniqueVectors
    {
        public List<DataSets> CreateDataSets(List<float[]> inputDataSets, List<float[]> outputDataSets) =>
            inputDataSets.Select((t, k) => new DataSets(t, outputDataSets[k])).ToList();

        public List<DataSets> CheckUniqueVectors(List<DataSets> dataSets, string path)
        {
            var vocabulary = new Hash2VecTextReader().Read(path);
            var newDataSets = new List<DataSets>(dataSets);
            int count = 0;

            using (var progressBar = new ProgressBar())
            {
                foreach (var representation in vocabulary.Words)
                {
                    var distanceList = vocabulary.Distance(representation, 3, 2).ToList();
                    var vectorsDuplicats = distanceList.AsParallel().Where(dis => dis.DistanceValue >= 0.95);

                    var vectorsEnumerable = vectorsDuplicats.AsParallel().SelectMany(vec =>
                        dataSets.Where(data => EqualsVectors(data.Vectors, vec.Representation.NumericVector))).ToList();

                    var vector = newDataSets.AsParallel()
                        .FirstOrDefault(vec => EqualsVectors(vec.Vectors, representation.NumericVector));

                    if (vector != null)
                    {
                        newDataSets.Remove(vector);
                        Parallel.ForEach(vectorsEnumerable, vec =>
                        {
                            for (var i = 0; i < vector.Ideals.Length; i++)
                                if (vector.Ideals[i] < vec.Ideals[i])
                                    vector.Ideals[i] = vec.Ideals[i];
                        });

                        newDataSets.Add(vector);
                    }

                    //   var enumerable = vectorsEnumerable.Select(data => data.Select(vec => newDataSets.Remove(vec)));

                    foreach (var data in vectorsEnumerable) newDataSets.Remove(data);

                    count++;
                    progressBar.Report((double) count / vocabulary.Words.Length);
                }
            }

            return newDataSets;
        }

        /// <summary>
        /// Парралельная обработка уникальных векторов по дистанции
        /// </summary>
        /// <param name="dataSets"></param>
        /// <param name="path"></param>
        /// <param name="flows">Кол-во потоков</param>
        /// <returns></returns>
        public IEnumerable<DataSets> CheckUniqueVectorsParallel(List<DataSets> dataSets, string path, int flows = 2)
        {
            var vocabulary = new Hash2VecTextReader().Read(path);
            var wordsLength = vocabulary.Words.Length;
            Data.NewDataSets = new List<DataSets>(dataSets);
            var threadList = new List<Thread>();

            int offset = wordsLength / flows;

            for (int i = 0, index = 1; i < wordsLength; i = i + offset, index++)
            {
                if (index == 2) i++;
                var uniqueVectors = new Core.UniqueVectors(vocabulary, dataSets, i, offset * index);
                threadList.Add(new Thread(uniqueVectors.GetUniqueVectors) {Name = $"Thread {index}"});
            }

            threadList.ForEach(thread => thread.Start());

            Wait(threadList);

            return Data.NewDataSets;
        }

        private void Wait(List<Thread> ThreadList)
        {
            while (true)
            {
                int WorkCount = 0;

                for (int i = 0; i < ThreadList.Count; i++)
                {
                    WorkCount += (ThreadList[i].IsAlive) ? 0 : 1;
                }

                if (WorkCount == ThreadList.Count) break;
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
