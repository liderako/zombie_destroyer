using Configs.Items;
using Gameplay.Stack.Item;
using UnityEngine;
using Utils;

namespace Gameplay.Stack.Interactions
{
    public class ItemForwarderReceivers : MonoBehaviour, IItemReceiver
    {
        [SerializeField] private Ref<IItemReceiver>[] values;
        public Transform Container => transform;
        private int current = -1;

        protected virtual void Awake()
        {
            if (TryGetComponent(out ItemForwarderItemStack itemStack))
            {
                itemStack.OnForwarderedEvent += ItemStackOnOnForwarderedEvent;
            }
        }

        private void ItemStackOnOnForwarderedEvent()
        {
            if (--current < 0)
                current = values.Length - 1;
        }

        public bool CanReceive
        {
            get
            {
                foreach (var value in values)
                    if (value.Value.CanReceive)
                        return true;

                return false;
            }
        }

        public bool Compatible(ItemConfigBase item)
        {
            foreach (var value in values)
                if (value.Value.Compatible(item))
                    return true;

            return false;
        }

        public bool Compatible(IItemContainerReadonly container, out ItemConfigBase item)
        {
            foreach (var value in values)
                if (value.Value.Compatible(container, out item))
                    return true;

            item = null;
            return false;
        }

        public Vector3 Position => transform.position;

        public int CanReceiveAmount(ItemConfigBase config)
        {
            int amount = 0;
            foreach (var value in values)
                amount += value.Value.CanReceiveAmount(config);

            return amount;
        }

        public virtual bool AddItem(ItemBase item, bool warp = false)
        {
            int i = 0;
            while (Next(ref i, out IItemReceiver receiver))
            {
                if (receiver.AddItem(item, warp))
                    return true;
            }

            return false;
        }

        public virtual void ItemStacked(ItemBase item) { }

        private bool Next(ref int i, out IItemReceiver source)
        {
            if (i++ >= values.Length)
            {
                source = null;
                return false;
            }

            if (++current >= values.Length)
                current = 0;

            source = values[current].Value;
            return true;
        }
    }
}