using Configs.Items;
using Gameplay.Stack.Item;
using Gameplay.Stack.Item.Utils;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace Gameplay.Stack.Interactions
{
    public class ItemDestructor : MonoBehaviour, IItemReceiver
    {
        [SerializeField] private bool filter;
        [SerializeField] private UnityEvent onItemStacked;

        [ShowIf(nameof(filter))]
        [SerializeField]
        private ItemConfigBase itemConfig;

        public bool Filtered => filter;
        public Transform Container => transform;

        public int CanReceiveAmount(ItemConfigBase value)
        {
            return Compatible(value) ? int.MaxValue : 0;
        }

        public bool CanReceive => true;
        public Vector3 Position => transform.position;

        public bool Compatible(ItemConfigBase item)
        {
            if (filter)
                return itemConfig == item;

            return true;
        }

        public bool Compatible(IItemContainerReadonly container, out ItemConfigBase item)
        {
            foreach (ItemConfigBase configBase in container.Items)
            {
                if (configBase == itemConfig)
                {
                    item = itemConfig;
                    return true;
                }
            }

            item = null;
            return false;
        }


        public bool AddItem(ItemBase item, bool warp = false)
        {
            if (!Compatible(item.Config))
                return false;

            AddItemUnsafe(item, warp);
            return true;
        }

        public void ItemStacked(ItemBase item)
        {
            onItemStacked.Invoke();
        }


        private void AddItemUnsafe(ItemBase item, bool warp)
        {
            item.ItemAnim.reachedAnim = ItemAnim.Anim.Disappear;
            item.SetStack(this, Vector3.zero, 1, warp);
        }
    }
}