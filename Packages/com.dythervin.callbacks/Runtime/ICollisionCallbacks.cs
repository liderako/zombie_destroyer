namespace Dythervin.Callbacks
{
    public interface ICollisionCallbacks : ICallbackBase
    {
        event CollisionHandler OnEnter;

        event CollisionHandler OnExit;
    }
}