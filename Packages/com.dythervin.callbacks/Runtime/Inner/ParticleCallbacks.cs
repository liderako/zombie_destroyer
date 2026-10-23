using System;
using UnityEngine;

namespace Dythervin.Callbacks.Inner
{
    [AddComponentMenu("")]
    internal sealed class ParticleCallbacks : CallbacksBase, IParticleCallbacks
    {
        private void OnParticleSystemStopped()
        {
            OnParticleSystemStop?.Invoke(this);
        }

        public event Action<IParticleCallbacks> OnParticleSystemStop;

        public override bool IsEmpty => OnParticleSystemStop == null;
    }
}