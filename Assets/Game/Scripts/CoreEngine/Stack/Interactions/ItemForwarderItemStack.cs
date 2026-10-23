using System;
using System.Collections.Generic;
using Configs.Items;
using Gameplay.Stack.Item;
using UnityEngine;
using Utils;

namespace Gameplay.Stack.Interactions
{
    public class ItemForwarderItemStack : MonoBehaviour, IItemSource, IItemStacks
    {
        public event Action OnForwarderedEvent;
        [SerializeField] private Ref<IItemStack>[] values;
        private IItemStack[] sources;

        private int current = -1;

        public Vector3 Position => transform.position;
        public IReadOnlyList<IItemStack> Stack => sources;

        public bool Empty
        {
            get
            {
                foreach (var value in sources)
                {
                    if (!value.Empty)
                        return false;
                }

                return true;
            }
        }

        private void Awake()
        {
            sources = new IItemStack[values.Length];
            for (int i = 0; i < sources.Length; i++)
            {
                sources[i] = values[i].Value;
            }
        }

        public bool TryPopItem(ItemConfigBase config, out ItemBase item)
        {
            int i = 0;
            while (Next(ref i, out IItemSource source))
            {
                if (source.TryPopItem(config, out item))
                {
                    OnForwarderedEvent?.Invoke();
                    return true;
                }
            }

            item = null;
            return false;
        }

        public bool TryPopItem(out ItemBase item)
        {
            int i = 0;
            while (Next(ref i, out IItemSource source))
            {
                if (source.TryPopItem(out item))
                {
                    OnForwarderedEvent?.Invoke();
                    return true;
                }
            }

            item = null;
            return false;
        }

        private bool Next(ref int i, out IItemSource source)
        {
            if (i++ >= sources.Length)
            {
                source = null;
                return false;
            }

            if (++current >= sources.Length)
                current = 0;

            source = sources[current];
            return true;
        }
    }
}