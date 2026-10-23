namespace Dythervin.Callbacks
{
    public interface ITriggerCallbacks : ICallbackBase
    {
        event TriggerHandler OnEnter;

        event TriggerHandler OnExit;
    }
}