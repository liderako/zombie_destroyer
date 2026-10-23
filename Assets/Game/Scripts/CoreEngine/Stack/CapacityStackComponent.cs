using System;
using System.Collections.Generic;
using Gameplay.Stack.Interactions;
using UnityEngine;
using Utils;

namespace Gameplay.Stack
{
    public class CapacityStackComponent : MonoBehaviour
    {
        [SerializeField] private Ref<IItemSource> source;
        [SerializeField] private int maxCapacity;
        
        public int CurrentCapacity { get; private set; }
        public bool IsStackFull { get; private set; }
        
        private List<ItemStack> itemStacks;
        private int stackSize;
        public event Action<bool> FullCapacityAction;
        // public event Action<int> UpdateCapacityAction;

        protected virtual void Awake()
        {
            itemStacks = new List<ItemStack>();
            for (int i = 0; i < transform.childCount; i++)
            {
                itemStacks.Add(transform.GetChild(i).GetComponent<ItemStack>());
            }
        }
        
        public void SetupCapacity(int newCapacity)
        {
            maxCapacity = newCapacity;
            SetStackSize(maxCapacity);
            CheckIfFull();
            // UpdateCapacityAction?.Invoke(CurrentCapacity);
        }

        public virtual void AddItem(int amount=1)
        {
            CurrentCapacity += amount;
            // UpdateCapacityAction?.Invoke(CurrentCapacity);
            CheckIfFull();
        }

        public virtual void RemoveItem(int amount=1)
        {
            CurrentCapacity -= amount;
            if (CurrentCapacity < 0)
            {
                CurrentCapacity = 0;
            }
            // UpdateCapacityAction?.Invoke(CurrentCapacity);
            CheckIfFull();
        }
        
        public void CheckIfFull()
        {
            if (CurrentCapacity >= stackSize && !IsStackFull)
            {
                IsStackFull = true;
                FullCapacityAction?.Invoke(IsStackFull);
            }
            else if (CurrentCapacity < stackSize && IsStackFull)
            {
                IsStackFull = false;
                FullCapacityAction?.Invoke(IsStackFull);
            }
        }
        
        public int GetStackSize()
        {
            return CurrentCapacity;
        }
        
        private void SetStackSize(int stackSizeInput=100)
        {
            stackSize = stackSizeInput;
            int count = itemStacks.Count;

            int baseCapacity = stackSize / count;
            int remainder = stackSize % count;

            for (int i = 0; i < count; i++)
            {
                int capacity = baseCapacity + (i < remainder ? 1 : 0);
                itemStacks[i].SetCapacity(capacity);
            }
        }
    }
}