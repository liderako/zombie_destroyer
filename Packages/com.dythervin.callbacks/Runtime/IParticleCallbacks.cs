using System;

namespace Dythervin.Callbacks
{
    public interface IParticleCallbacks : ICallbackBase
    {
        public event Action<IParticleCallbacks> OnParticleSystemStop;
    }
}