using System;
using System.Collections.Generic;
using UnityEngine;

namespace Collections
{
    [Serializable]
    public class SerializedDictionary<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver
    {
        [SerializeField]
        [HideInInspector]
        private TKey[] mKeys;

        [SerializeField]
        [HideInInspector]
        private TValue[] mValues;

        public void OnBeforeSerialize()
        {
            if (Count == 0)
                return;
            mKeys = new TKey[Count];
            mValues = new TValue[Count];

            int i = 0;
            foreach (var pair in this)
            {
                mKeys[i] = pair.Key;
                mValues[i++] = pair.Value;
            }
        }

        public void OnAfterDeserialize()
        {
            Clear();
            for (int i = 0; i < mKeys.Length; i++)
            {
                Add(mKeys[i], mValues[i]);
            }

            mKeys = null;
            mValues = null;
        }
    }
}