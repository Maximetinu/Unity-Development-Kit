using System;
using System.Collections.Generic;

namespace UDK
{
    public static class IListExt
    {
        private static readonly Random Random = new();

        public static void Shuffle<T>(this IList<T> list)
        {
            list.Shuffle(Random);
        }

        public static void Shuffle<T>(this IList<T> list, Random random)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = random.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

        public static T RandomInstance<T>(this IList<T> list)
        {
            return list.RandomInstance(Random);
        }

        public static T RandomInstance<T>(this IList<T> list, Random random)
        {
            var index = random.Next(list.Count);
            return list[index];
        }
    }
}
