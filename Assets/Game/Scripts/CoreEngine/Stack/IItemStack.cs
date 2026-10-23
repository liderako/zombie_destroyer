using System.Collections.Generic;
using Configs.Items;
using Gameplay.Stack.Interactions;
using Gameplay.Stack.Item;

namespace Gameplay.Stack
{
    public interface IItemContainerReadonly
    {
        public event System.Action OnChanged;
        public event System.Action<ItemBase, bool> OnChangedDetailed;
        public IEnumerable<ItemConfigBase> Items { get; }
        int GetItemCount(ItemConfigBase config);
        int Count { get; }
        int Capacity { get; }
    }

    public interface IItemStack : IItemContainerReadonly, IItemReceiver, IItemSource
    {
    }
}