namespace Dythervin.Callbacks
{
    public interface ICallbackBase : IGameObjectAttached
    {
        bool IsEmpty { get; }
    }
}