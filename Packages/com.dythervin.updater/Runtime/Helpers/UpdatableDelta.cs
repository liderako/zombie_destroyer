using System;

namespace Dythervin.UpdateSystem.Helpers
{
    public class UpdatableDelta : IUpdatableDelta, IUpdatableListenerDelta
    {
        public event Action<float> OnUpdate;

        public bool enabled
        {
            get => this.GetUpdater();
            set => this.SetUpdater(value);
        }

        public UpdatableDelta(Action<float> onUpdate = null, bool enabled = false)
        {
            OnUpdate = onUpdate;
            if (enabled)
                this.enabled = true;
        }

        void IUpdatableDelta.OnUpdate(float deltaTime)
        {
            OnUpdate?.Invoke(deltaTime);
        }
    }
}