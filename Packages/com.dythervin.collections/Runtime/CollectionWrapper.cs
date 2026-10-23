using System;
using System.Collections.Generic;
#if ODIN_INSPECTOR && UNITY_EDITOR
using Sirenix.OdinInspector;
#endif
using UnityEngine;

namespace Dythervin.Collections
{
#if ODIN_INSPECTOR && UNITY_EDITOR
    [HideLabel]
#endif
    [Serializable]
    public class CollectionWrapper<TCollection, T> : ISerializationCallbackReceiver
        where TCollection : class, ICollection<T>, new()
    {
        // ReSharper disable once InconsistentNaming
        [SerializeReference] private T[] array;

        [NonSerialized]
        private TCollection collection;

        public CollectionWrapper()
        {
            collection = new TCollection();
        }

        public CollectionWrapper(TCollection collection)
        {
            this.collection = collection;
        }

        void ISerializationCallbackReceiver.OnBeforeSerialize()
        {
        }

        void ISerializationCallbackReceiver.OnAfterDeserialize()
        {
            if (array == null)
                return;

            collection.Clear();
            foreach (T t in array)
            {
                collection.Add(t);
            }
        }
    }
}