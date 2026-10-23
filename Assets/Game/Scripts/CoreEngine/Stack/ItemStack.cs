using System;
using System.Collections.Generic;
using Configs.Items;
using Dythervin.ObjectPool.Component.Global;
using Gameplay.Stack.Interactions;
using Gameplay.Stack.Item;
using Sirenix.OdinInspector;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.Events;
using Utils.Extensions;

namespace Gameplay.Stack
{
    public class ItemStack : MonoBehaviour, IItemStack
    {
        private const float Spacing = 0.05f;
        [SerializeField] private bool unlimitedCapacity;

        [HideIf(nameof(unlimitedCapacity))]
        [SerializeField]
        private int capacity = 1;

        [SerializeField] public UnityEvent onItemStacked;
        public event Action<ItemBase> OnItemStackedRotate;
        [SerializeField] private Transform container;

        [ShowInInspector, ReadOnly] private readonly List<ItemBase> items = new List<ItemBase>();
        [ShowInInspector, ReadOnly] private readonly Dictionary<ItemConfigBase, int> itemCount = new Dictionary<ItemConfigBase, int>();

        private Vector3 currentHeight;
        
        private bool visualEnabled = true;
        public event System.Action<ItemConfigBase, bool> OnItemTypeChanged;
        public event System.Action<ItemBase, bool> OnChangedDetailed;
        public event System.Action OnChanged;
        public event System.Action<bool> OnVisualChanged;

        public bool Empty => Count == 0;
        public IEnumerable<ItemConfigBase> Items => itemCount.Keys;
        public int Capacity => unlimitedCapacity ? int.MaxValue : capacity;
        public bool Filtered { get; protected set; }
        public Vector3 Position => transform.position;
        [ShowInInspector, ReadOnly] public int Count => items.Count;
        protected int Free => Capacity - Count;
        public Transform Container => container;
        public virtual bool CanReceive => items.Count < Capacity;
        public Vector3 TopmostPoint => transform.TransformPoint(currentHeight);

        private void OnEnable()
        {
            ItemsVisualEnabled = true;
        }
        
#if UNITY_EDITOR
        [Button]
        private void SetupComponents()
        {
            if (container == null)
            {
                container = transform;
                EditorUtility.SetDirty(this);
            }
        }
#endif

        private void OnDisable()
        {
#if UNITY_EDITOR
            if (Common.ApplicationExt.Quitting)
                return;
#endif
            ItemsVisualEnabled = false;
        }

        public void SetCapacity(int cap)
        {
            capacity = cap;
        }

        public virtual int CanReceiveAmount(ItemConfigBase value)
        {
            return Compatible(value) ? Free : 0;
        }

        public bool Compatible(ItemConfigBase item)
        {
            return CanReceive && CompatibleSingleItem(item);
        }

        protected virtual bool CompatibleSingleItem(ItemConfigBase item)
        {
            return true;
        }

        public bool Compatible(IItemContainerReadonly container, out ItemConfigBase item)
        {
            if (CanReceive)
            {
                foreach (ItemConfigBase configBase in container.Items)
                {
                    if (CompatibleSingleItem(configBase) && CanReceiveAmount(configBase) > 0)
                    {
                        item = configBase;
                        return true;
                    }
                }
            }

            item = null;
            return false;
        }

        public virtual bool AddItem(ItemBase item, bool warp = false)
        {
            if (Compatible(item.Config) && CanReceiveAmount(item.Config) > 0)
            {
                AddItemUnsafe(item, warp);
                return true;
            }

            return false;
        }
        void IItemReceiver.ItemStacked(ItemBase item)
        {
            onItemStacked.Invoke();
            OnItemStackedRotate?.Invoke(item);
        }

        protected void AddItemUnsafe(ItemBase item, bool warp)
        {
            AddItemUnsafeSilently(item, warp);
            ItemConfigBase config = item.Config;
            if (itemCount.ContainsKey(config))
                itemCount[config]++;
            else
            {
                itemCount.Add(config, 1);
                OnItemTypeChanged?.Invoke(config, true);
            }

            OnChangedDetailed?.Invoke(item, true);
            OnChanged?.Invoke();
        }

        private void AddItemUnsafeSilently(ItemBase item, bool warp)
        {
            if (customBehavior)
            {
                currentHeight.y = Mathf.Clamp(currentHeight.y, 0, maxHeight);
                item.SetStack(this, currentHeight, 1f + 0.075f * (1f - items.Count / (float)Capacity), warp);
                currentHeight.y = Mathf.Clamp(currentHeight.y + item.Height, 0, maxHeight);
            }
            else
            {
                currentHeight.y += Spacing;
                item.SetStack(this, currentHeight, 1f + 0.075f * (1f - items.Count / (float)Capacity), warp);   
                currentHeight.y += item.Height;
            }
            items.Add(item);
        }
        
        [SerializeField] private bool customBehavior;
        [SerializeField] private float maxHeight;
        
        public int GetItemCount(ItemConfigBase config)
        {
            itemCount.TryGetValue(config, out int value);
            return value;
        }

        public bool TryPopItem(out ItemBase item)
        {
            if (items.Count > 0)
            {
                if (items[items.Count - 1].Moving)
                {
                    item = null;
                    return false;
                }
                item = PopItemByIndex(items.Count - 1);
                return true;
            }
            item = null;
            return false;
        }

        public bool TryPopItem(ItemConfigBase config, out ItemBase item)
        {
            if (enabled && GetItemCount(config) > 0)
            {
                item = PopItemByConfig(config);
                return true;
            }

            item = null;
            return false;
        }
        
        public void Clear()
        {
            while (TryPopItem(out ItemBase item))
                GlobalPools.ReleaseOrDestroy(item);
        }

        private ItemBase PopItemByConfig(ItemConfigBase config)
        {
            for (int i = items.Count - 1; i >= 0; i--)
            {
                if (items[i].Config == config)
                {
                    return PopItemByIndex(i);
                }
            }
            return null;
        }

        private ItemBase PopItemByIndex(int index)
        {
            ItemBase item = items.PopAt(index);
            ItemConfigBase config = item.Config;
            item.transform.SetParent(null);
            if (--itemCount[config] == 0)
            {
                itemCount.Remove(config);
                OnItemTypeChanged?.Invoke(config, false);
            }
            Shrink(index, new Vector3(0, -(item.Height * item.transform.localScale.y + Spacing), 0));
            OnChangedDetailed?.Invoke(item, false);
            OnChanged?.Invoke();
            return item;
        }


        private void Shrink(int index, in Vector3 yDelta)
        {
            for (int i = index; i < items.Count; i++)
            {
                ItemBase item = items[i];
                item.AddStackOffset(yDelta);
            }

            currentHeight.y += yDelta.y;
        }

        public bool ItemsVisualEnabled
        {
            get => visualEnabled;
            private set
            {
                if (visualEnabled == value)
                    return;
                foreach (ItemBase item in items)
                {
                    if (item != null)
                    {
                        item.VisualEnabled = value;
                    }
                }
                visualEnabled = value;
                OnVisualChanged?.Invoke(value);
            }
        }

        public bool GetIrrelevantItem(out ItemConfigBase item)
        {
            foreach (ItemConfigBase config in Items)
            {
                if (CompatibleSingleItem(config))
                    continue;

                item = config;
                return true;
            }

            item = null;
            return false;
        }
    }
}