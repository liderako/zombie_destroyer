using System;
using UnityEngine;

namespace Dythervin.Callbacks.Inner
{
    [AddComponentMenu("")]
    internal sealed class Callbacks : CallbacksBase, ICallbacks
    {
        public event Action<ICallbacks> OnDisabled;

        public event Action<ICallbacks, bool> OnActiveChanged;

        public event Action<ICallbacks> OnEnabled;

        public event Action<ICallbacks> OnDestroyed;

        public override bool IsEmpty => OnEnabled == null && OnDisabled == null && OnActiveChanged == null && OnDisabled == null;

        private void OnEnable()
        {
            OnEnabled?.Invoke(this);
            OnActiveChanged?.Invoke(this, true);
        }

        private void OnDisable()
        {
            OnDisabled?.Invoke(this);
            OnActiveChanged?.Invoke(this, false);
        }

        private void OnDestroy()
        {
            OnDestroyed?.Invoke(this);
        }
    }
}