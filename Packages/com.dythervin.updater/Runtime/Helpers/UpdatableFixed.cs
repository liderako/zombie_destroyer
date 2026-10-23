using System;

namespace Dythervin.UpdateSystem.Helpers
{
    public class UpdatableFixed : IUpdatableFixed, IUpdatableListener
    {
        public event Action OnUpdate;

        public bool enabled
        {
            get => this.GetFixedUpdater();
            set => this.SetFixedUpdater(value);
        }

        public UpdatableFixed(Action onUpdate = null, bool enabled = false)
        {
            OnUpdate = onUpdate;
            if (enabled)
                this.enabled = true;
        }

        void IUpdatableFixed.OnUpdate()
        {
            OnUpdate?.Invoke();
        }
    }
}