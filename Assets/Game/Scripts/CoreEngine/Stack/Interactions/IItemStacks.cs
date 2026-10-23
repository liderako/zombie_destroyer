using System.Collections.Generic;

namespace Gameplay.Stack.Interactions
{
    public interface IItemStacks
    {
        public IReadOnlyList<IItemStack> Stack { get; }
    }
}