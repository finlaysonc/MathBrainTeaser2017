using System;
using System.Collections.Generic;
using System.Linq;

namespace Countdown2017
{
    public static class Combinatorics
    {
        public static IEnumerable<IEnumerable<T>> GetPowerSet<T>(this IReadOnlyList<T> list)
        {
            return from m in Enumerable.Range(0, 1 << list.Count)
                   select
                   from i in Enumerable.Range(0, list.Count)
                   where (m & (1 << i)) != 0
                   select list[i];
        }

        public static IEnumerable<T[]> GetVariations<T>(this IReadOnlyList<T> list)
        {
            return from s in GetPowerSet(list).Skip(1)
                   select s.ToArray();
        }

        //public static IEnumerable<T[]> GetCombinations<T>(this IEnumerable<T> source, int n)
        //{
        //    if (n == 0)
        //        yield return new T[0];


        //    int count = 1;
        //    foreach (T item in source)
        //    {
        //        foreach (var innerSequence in source.Skip(count).GetCombinations(n - 1))
        //        {
        //            yield return innerSequence.Prepend(item);
        //        }
        //        count++;
        //    }
        //}


        public static IEnumerable<T[]> GetPermutations<T>(this IEnumerable<T> items)
        {
            if (items.Count() > 1)
            {
                return items.SelectMany(item => GetPermutations(items.Where(i => !i.Equals(item))),
                                        (item, permutation) => permutation.Prepend(item));
            }
            return new[] {items.ToArray()};
        }


        public static T[] Prepend<T>(this T[] rest, T first)
        {
            T[] result = new T[rest.Length + 1];
            result[0] = first;
            if (rest.Length > 0)
            {
                Array.Copy(rest, 0, result, 1, rest.Length);
            }
            return result;
        }
    }
}