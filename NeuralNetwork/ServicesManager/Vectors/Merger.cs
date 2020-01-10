using System.Collections.Generic;
using System.Linq;
using NeuralNetwork.Extensions;

namespace NeuralNetwork.ServicesManager.Vectors
{
    public class Merger
    {
        public List<List<double[]>> MergeItems(List<double[]> inputDataSets, List<double[]> outputDataSets)
        {
            var newInputDataSets = new List<double[]>();
            var newOutputDataSets = new List<double[]>();

            // Все индексы слов, которые повторяются:
            var repeatedWordsIndexes = new List<int>();
            using (var progressBar = new ProgressBar())
            {
                for (int i = 0, index = 0; i < inputDataSets.Count; i++, index = 0)
                {
                    // Поиск индексов всех повторяющихся элементов:
                    while (index != -1)
                    {
                        index = inputDataSets.FindIndex(inputDataSets[i], i);

                        // Если что-то найдено и найденный элемент не является обрабатываемым
                        if (index != -1 && index != i)
                        {
                            repeatedWordsIndexes.Add(index);
                            inputDataSets.RemoveAt(index);
                        }
                        else
                        {
                            break;
                        }
                    }

                    // Слияние выходных векторов всех найденых элементов:
                    var outputVector = new double[outputDataSets[0].Length];
                    if (repeatedWordsIndexes.Count != 0)
                    {
                        outputVector = repeatedWordsIndexes.Aggregate(outputVector,
                            (current, t) => current.VectorSum(outputDataSets[t]));
                    }

                    // Добавление уже уникального элемента в общии коллекции:
                    newInputDataSets.Add(inputDataSets[i]);
                    newOutputDataSets.Add(repeatedWordsIndexes.Count != 0
                        ? outputVector.VectorSum(outputDataSets[i]) : outputDataSets[i]);

                    // Удаление повторяющихся элементов:
                    foreach (var repeatedWordsIndex in repeatedWordsIndexes)
                        outputDataSets.RemoveAt(repeatedWordsIndex);

                    repeatedWordsIndexes.Clear();

                    progressBar.Report((double)i / inputDataSets.Count);
                }
            }

            return new List<List<double[]>> { newInputDataSets, newOutputDataSets };
        }
    }
}
