namespace Dythervin.Callbacks
{
    public interface ITriggerStayCallback : ICallbackBase
    {
        public event TriggerStayHandler OnStay;
        public void DestroyIfEmpty();
    }
}