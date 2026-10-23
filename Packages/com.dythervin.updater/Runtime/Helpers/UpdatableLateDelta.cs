using System;

namespace Dythervin.UpdateSystem.Helpers
{
    public class UpdatableLateDelta : IUpdatableLateDelta, IUpdatableListenerDelta
    {
        public event Action<float> OnUpdate;

        public bool enabled
        {
            get => this.GetLateUpdater();
            set => this.SetLateUpdater(value);
        }

        public UpdatableLateDelta(Action<float> onUpdate = null, bool enabled = false)
        {
            OnUpdate = onUpdate;
            if (enabled)
                this.enabled = true;
        }

        void IUpdatableLateDelta.OnUpdate(float deltaTime)
        {
            OnUpdate?.Invoke(deltaTime);
        }
    }
}