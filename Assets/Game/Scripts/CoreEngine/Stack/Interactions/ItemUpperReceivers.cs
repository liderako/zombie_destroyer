using Configs.Items;
using Gameplay.Stack;
using Gameplay.Stack.Interactions;
using Gameplay.Stack.Item;
using UnityEngine;
using Utils;
using Random = UnityEngine.Random;

namespace _GameData_.Scripts.Stack.Interactions
{
    public class ItemUpperReceivers : MonoBehaviour, IItemReceiver
    {
        [SerializeField] private Ref<IItemReceiver>[] values;
        [SerializeField] private int Height;

        private int indexCurrentHeight;
        public Transform Container => transform;
        private int current = -1;

        private void Awake()
        {
            if (TryGetComponent(out ItemForwarderItemStack itemStack))
            {
                itemStack.OnForwarderedEvent += ItemStackOnOnForwarderedEvent;
            }
        }

        private void OnEnable()
        {
            for (int i = 0; i < values.Length; i++)
            {
                if (values[i].Value is ItemStack itemStack)
                {
                    itemStack.OnItemStackedRotate += RotateItem;
                }
            }
        }
        
        private void OnDisable()
        {
            for (int i = 0; i < values.Length; i++)
            {
                if (values[i].Value is ItemStack itemStack)
                {
                    itemStack.OnItemStackedRotate -= RotateItem;
                }
            }
        }
        
        protected virtual void RotateItem(ItemBase itemBase)
        {
            itemBase.transform.localRotation = Quaternion.Euler(itemBase.Config.originRotate.x, itemBase.Config.originRotate.y, itemBase.Config.originRotate.z);   
            if (itemBase.Config.MaxRotateForEveryCoordinates.y != 0)
            {
                itemBase.transform.localRotation = Quaternion.Euler(itemBase.Config.originRotate.x, 
                    Random.Range(itemBase.Config.originRotate.y - itemBase.Config.MaxRotateForEveryCoordinates.y, itemBase.Config.originRotate.y + itemBase.Config.MaxRotateForEveryCoordinates.y),
                    itemBase.Config.originRotate.z);   
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

        public bool AddItem(ItemBase item, bool warp = false)
        {
            int i = 0;
            while (Next(ref i, out IItemReceiver receiver))
            {
                if (receiver.AddItem(item, warp))
                    return true;
            }

            return false;
        }

        public void ItemStacked(ItemBase item)
        {
        }

        private bool Next(ref int i, out IItemReceiver source)
        {
            if (i++ >= values.Length)
            {
                source = null;
                return false;
            }
            
            if (current == -1)
            {
                current = 0;
            }

            source = values[current].Value;

            indexCurrentHeight++;

            if (indexCurrentHeight >= Height)
            {
                current++;
                indexCurrentHeight = 0;
            }
            
            if (current >= values.Length)
            {
                current = 0;   
            }
            
            return true;
        }
    }
}