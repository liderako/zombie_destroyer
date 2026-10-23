using UnityEngine;

namespace Dythervin.Callbacks.Inner
{
    [AddComponentMenu("")]
    internal class CollisionCallbacks : CallbacksBase, ICollisionCallbacks
    {
        public event CollisionHandler OnEnter;

        public event CollisionHandler OnExit;

        public override bool IsEmpty => OnEnter == null && OnExit == null;

        private void OnCollisionEnter(Collision other)
        {
            OnEnter?.Invoke(this, other);
        }

        private void OnCollisionExit(Collision other)
        {
            OnExit?.Invoke(this, other);
        }
    }
}