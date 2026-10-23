using Configs.Items;
using Gameplay.Stack.Item;

namespace Gameplay.Stack.Interactions
{
    public interface IItemSource : IPositioned
    {
        public bool Empty { get; }
        public bool TryPopItem(ItemConfigBase config, out ItemBase item);
        public bool TryPopItem(out ItemBase item);
    }
}