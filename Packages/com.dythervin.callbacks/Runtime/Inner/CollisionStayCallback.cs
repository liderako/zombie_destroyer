using UnityEngine;

namespace Dythervin.Callbacks.Inner
{
    [AddComponentMenu("")]
    internal class CollisionStayCallback : CallbacksBase, ICollisionStayCallback
    {
        public event CollisionStayHandler OnStay;

        public override bool IsEmpty => OnStay == null;

        private void OnCollisionStay(Collision other)
        {
            OnStay?.Invoke(this, other);
        }
    }
}