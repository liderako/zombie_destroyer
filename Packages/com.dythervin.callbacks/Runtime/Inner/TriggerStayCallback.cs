using UnityEngine;

namespace Dythervin.Callbacks.Inner
{
    [AddComponentMenu("")]
    internal class TriggerStayCallback : CallbacksBase, ITriggerStayCallback
    {
        public event TriggerStayHandler OnStay;

        public override bool IsEmpty => OnStay == null;

        public void DestroyIfEmpty()
        {
            if (OnStay == null)
                Destroy(this);
        }

        private void OnTriggerStay(Collider other)
        {
            OnStay?.Invoke(this, other);
        }
    }
}