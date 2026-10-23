using Configs.Items;
using Gameplay.Stack.Item;
using UnityEngine;
using Utils;

namespace Gameplay.Stack.Interactions
{
    public class ItemForwarderReceiver : MonoBehaviour, IItemReceiver
    {
        [SerializeField] private Ref<IItemReceiver> value;
        public bool CanReceive => value.Value.CanReceive;
        public Transform Container => transform;
        public bool Compatible(ItemConfigBase item)
        {
            return value.Value.Compatible(item);
        }

        public bool Compatible(IItemContainerReadonly container, out ItemConfigBase item)
        {
            return value.Value.Compatible(container, out item);
        }

        public Vector3 Position => transform.position;

        public int CanReceiveAmount(ItemConfigBase value)
        {
            return this.value.Value.CanReceiveAmount(value);
        }

        public bool AddItem(ItemBase item, bool warp = false)
        {
            return value.Value.AddItem(item, warp);
        }

        public void ItemStacked(ItemBase item) { }
    }
}