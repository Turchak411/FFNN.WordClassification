using System;
using System.Linq;
using System.Collections.Generic;

namespace NeuralNetwork.Extensions
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<TSource> Duplicates<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> selector)
        {
            var grouped = source.GroupBy(selector);
            var moreThan1 = grouped.Where(i => i.IsMultiple());
            return moreThan1.SelectMany(i => i);
        }

        public static IEnumerable<TSource> Duplicates<TSource, TKey>(this IEnumerable<TSource> source) => source.Duplicates(i => i);

        public static bool IsMultiple<T>(this IEnumerable<T> source)
        {
            var enumerator = source.GetEnumerator();
            return enumerator.MoveNext() && enumerator.MoveNext();
        }

       
        public static int FindIndex(this IEnumerable<double[]> items1, double[] items2, int itemsIndex)
        {
            var enumerable = items1 as double[][] ?? items1.ToArray();

            for (var index = 0; index < enumerable.Length; index++)
                if (itemsIndex != index && !items2.Where((t, k) 
                        => !enumerable[index][k].Equals(t)).Any()) return index;
            return -1;
        }

        public static int FindIndex(this IEnumerable<double[]> items1, KeyValuePair<int, double[]> keyValue)
        {
            var enumerable = items1 as double[][] ?? items1.ToArray();

            for (var index = 0; index < enumerable.Length; index++)
                if (keyValue.Key != index && !keyValue.Value.Where((t, k)
                        => !enumerable[index][k].Equals(t)).Any()) return index;
            return -1;
        }
    }
}
