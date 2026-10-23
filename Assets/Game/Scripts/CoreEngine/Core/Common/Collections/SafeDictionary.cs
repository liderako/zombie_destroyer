using System.Collections.Generic;
using Gameplay.Stack.Item.Utils;

namespace Common.Collections
{
    public class SafeDictionary<TKey, TValue> : Dictionary<TKey, TValue>, IIndexer<TKey, TValue>
    {
        public new TValue this[TKey key]
        {
            get => ContainsKey(key) ? base[key] : default;
            set
            {
                if (ContainsKey(key))
                    base[key] = value;
                else
                    Add(key, value);
            }
        }
    }

    public class SafeIntDictionary<TKey> : Dictionary<TKey, int>, IIndexer<TKey, int>
    {
        public new int this[TKey key]
        {
            get => TryGetValue(key, out int value) ? value : default;
            set
            {
                if (ContainsKey(key))
                {
                    if (value == 0)
                        Remove(key);
                    else
                        base[key] = value;
                }
                else if (value != 0)
                    Add(key, value);
            }
        }
    }
}