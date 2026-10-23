using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Utils.Extensions;

namespace Collections
{
#if ODIN_INSPECTOR
    using Sirenix.OdinInspector;
#endif

    public interface IRandomElement
    {
        public uint Point { get; }
    }


    [System.Serializable]
    public class RandomList<T> : ISerializationCallbackReceiver
    {
        [System.Serializable]
        public struct Element : IRandomElement
        {
            public T value;

            [SerializeField]
            [Min(1)]
            private uint point;

#if ODIN_INSPECTOR
            [ShowInInspector]
            [ReadOnly]
#endif
            [System.NonSerialized]
            public float Probability;

            public Element(T value, uint point)
            {
                this.value = value;
                this.point = math.max(point, 1);
                Probability = 0;
            }

            public uint Point => point;

            public static implicit operator T(in Element value)
            {
                return value.value;
            }
        }

        [SerializeField]
#if ODIN_INSPECTOR
        [OnCollectionChanged(nameof(ShowProbabilities))]
#endif
        private List<Element> values;

        public IReadOnlyList<Element> Values => values;

        private readonly List<uint> probValues = new List<uint>();
        private readonly Dictionary<uint, List<int>> dictIndexes = new Dictionary<uint, List<int>>();

        private uint total;

        public void OnBeforeSerialize() { }

        public void OnAfterDeserialize()
        {
#if UNITY_EDITOR
            ShowProbabilities();
#else
            Update();
#endif
        }

        public RandomList()
        {
            values = new List<Element>();
        }

#if ODIN_INSPECTOR
        [Button]
#endif
        private void Update()
        {
            Reset();
            for (int i = 0; i < values.Count; i++)
            {
                AddItem(values[i].Point, i, out _);
            }
        }

#if ODIN_INSPECTOR
        [Button]
#endif
        private void ShowProbabilities()
        {
            Update();
            for (int i = 0; i < values.Count; i++)
            {
                var value = values[i];
                value.Probability = value.Point / (float)total;
                values[i] = value;
            }
        }

#if ODIN_INSPECTOR
        [Button]
#endif
        public void Add(Element value)
        {
            AddItem(value.Point, values.Count, out bool newValue);
            values.Add(value);

            if (newValue)
                probValues.Sort();
        }

#if ODIN_INSPECTOR
        [Button]
#endif
        public void Remove(Element value)
        {
            int index = values.IndexOf(value);
            if (index < 0)
                return;

            uint point = value.Point;
            var indexes = dictIndexes[point];
            total -= point;
            indexes.Remove(index);
            if (indexes.Count == 0)
            {
                dictIndexes.Remove(point);
                probValues.Remove(point);
            }
        }

        public void Clear()
        {
            values.Clear();
            Reset();
        }

        private void Reset()
        {
            dictIndexes.Clear();
            probValues.Clear();
            total = 0;
        }

        private void AddItem(uint point, int index, out bool needSort)
        {
            needSort = !dictIndexes.TryGetValue(point, out var indexes);
            if (needSort)
            {
                indexes = new List<int>();
                dictIndexes.Add(point, indexes);
                probValues.Add(point);
            }

            total += point;
            indexes.Add(index);
        }

        private void RemoveItem(uint point, int index)
        {
            if (!dictIndexes.TryGetValue(point, out var list))
            {
                list = new List<int>();
                dictIndexes.Add(point, list);
                probValues.Add(point);
            }

            total -= point;
            list.Remove(index);
        }

        public T Random => GetRandom(UnityEngine.Random.value);

        /// <param name="value">[0:1]</param>
#if ODIN_INSPECTOR
        [Button]
        public Element GetRandom([PropertyRange(0, 1)] float value)
#else
        public Element GetRandom(float value)
#endif
        {
            float prob = value * total;
            uint prevValue = 0;
            foreach (uint point in probValues)
            {
                prevValue += point * (uint)dictIndexes[point].Count;

                if (prob <= prevValue)
                    return values[dictIndexes[point].GetRandom()];
            }

            throw new System.ArgumentOutOfRangeException();
        }
    }
}