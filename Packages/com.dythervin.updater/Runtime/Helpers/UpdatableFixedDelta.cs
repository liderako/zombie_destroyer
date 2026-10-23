using System;

namespace Dythervin.UpdateSystem.Helpers
{
    public class UpdatableFixedDelta : IUpdatableFixedDelta, IUpdatableListenerDelta
    {
        public event Action<float> OnUpdate;

        public bool enabled
        {
            get => this.GetFixedUpdater();
            set => this.SetFixedUpdater(value);
        }

        public UpdatableFixedDelta(Action<float> onUpdate = null, bool enabled = false)
        {
            OnUpdate = onUpdate;
            if (enabled)
                this.enabled = true;
        }

        void IUpdatableFixedDelta.OnUpdate(float deltaTime)
        {
            OnUpdate?.Invoke(deltaTime);
        }
    }
}