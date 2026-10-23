using System.Collections;
using System.Collections.Generic;
using Common;
using Configs.Items;
using Gameplay.Stack.Item;
using Gameplay.Stack.Pool;
using Sirenix.OdinInspector;
using UnityEngine;
using Utils.Extensions;

namespace Gameplay.Stack.Interactions
{
    public class ItemGenerator : MonoBehaviour, IItemSource
    {
        [OnValueChanged(nameof(OnConfigChanged))]
        [SerializeField]
        private ItemConfigBase config;

        [SerializeField] private float spawnInterval = 0.1f;

        private ItemBase currentItem;

        private ItemConfigBase[] items;
        public IEnumerable<ItemConfigBase> Items => items;

        public Vector3 Position => transform.position;
        public bool Empty => false;
        private bool hasItem;

        private void Awake()
        {
            items = new[] { config };
        }

        private IEnumerator Start()
        {
            while (true)
            {
                while (hasItem)
                    yield return null;

                yield return Waiters.Wait(spawnInterval, true);

                currentItem = ItemPool.Instance.ObjectPool.Get();
                currentItem.Load(config);
                hasItem = true;
            }
        }

        public bool TryPopItem(ItemConfigBase config, out ItemBase item)
        {
            if (config == this.config && currentItem)
            {
                item = currentItem;
                currentItem = null;
                hasItem = false;
                return true;
            }

            item = null;
            return false;
        }

        public bool TryPopItem(out ItemBase item)
        {
            return TryPopItem(config, out item);
        }


        private void OnConfigChanged()
        {
#if UNITY_EDITOR
            if (!config)
            {
                name = nameof(ItemGenerator);
                if (currentItem)
                    DestroyImmediate(currentItem.gameObject);
            }
            else
            {
                name = $"{nameof(ItemGenerator)} - {config.name}";
            }

            this.Dirty();
#endif
        }
    }
}