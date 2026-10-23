using Configs.Items;
using Gameplay.Stack.Item;
using UnityEngine;

namespace Gameplay.Stack.Interactions
{
    public interface IItemFilter
    {
        bool Compatible(ItemConfigBase item);
        bool Compatible(IItemContainerReadonly container, out ItemConfigBase item);
    }

    public interface IItemReceiver : IItemFilter, IPositioned
    {
        int CanReceiveAmount(ItemConfigBase value);
        bool CanReceive { get; }
        bool AddItem(ItemBase item, bool warp = false);
        void ItemStacked(ItemBase item);
        Transform Container { get; } }
    
}