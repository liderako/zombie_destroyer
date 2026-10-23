namespace Gameplay.Stack.Item.Utils
{
    public interface IIndexer<in TKey, TValue>
    {
        TValue this[TKey key] { get; set; }
    }
}