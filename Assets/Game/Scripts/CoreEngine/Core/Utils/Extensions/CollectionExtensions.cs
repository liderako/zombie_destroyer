using System.Collections.Generic;
using Random = UnityEngine.Random;

namespace Utils.Extensions
{
    public static class CollectionExtensions
    {
        private static readonly List<uint> probabilities = new List<uint>();
        private static readonly List<int> probabilitiesIndexes;

        public static T GetRandom<T>(this IList<T> list)
        {
            return list[list.GetRandomIndex()];
        }

        public static T GetRandom<T>(this IList<T> list, out int index)
        {
            index = list.GetRandomIndex();
            T item = list[index];
            return item;
        }

        public static int GetRandomIndex<T>(this IList<T> list)
        {
            return Random.Range(0, list.Count);
        }

        public static T PopRandom<T>(this IList<T> list)
        {
            int index = list.GetRandomIndex();
            T item = list[index];
            list.RemoveAt(index);
            return item;
        }

        public static T PopLast<T>(this IList<T> list)
        {
            return PopLast(list, out _);
        }
        
        public static T Last<T>(this IList<T> list)
        {
            return list[list.Count -1];
        }

        public static T PopLast<T>(this IList<T> list, out int index)
        {
            index = list.Count - 1;
            T item = list[index];
            list.RemoveAt(index);
            return item;
        }

        public static T PopAt<T>(this List<T> list, int index)
        {
            T item = list[index];
            list.RemoveAt(index);
            return item;
        }

#if !NETSTANDARD2_1_OR_GREATER
        public static bool TryPop<T>(this Stack<T> stack, out T value)
        {
            bool has = stack.Count > 0;
            value = has ? stack.Pop() : default;
            return has;
        }
#endif
    }
    
    public static class ArrayExtensions
    {
        /// <summary>
        /// Перемешивает элементы массива «на месте» (Fisher–Yates).
        /// </summary>
        public static void Shuffle<T>(this T[] array, int length)
        {
            int count = length;
            int last  = count - 1;
    
            for (int i = 0; i < last; i++)
            {
                int r = UnityEngine.Random.Range(i, count);
                (array[i], array[r]) = (array[r], array[i]);
            }
        }
        
        public static void Shuffle<T>(this IList<T> list)
        {
            System.Random rng = new System.Random();
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                (list[n], list[k]) = (list[k], list[n]);
            }
        }
    }
}