using System;

namespace Dythervin.UpdateSystem.Helpers
{
    public class Updatable : IUpdatable, IUpdatableListener
    {
        public event Action OnUpdate;

        public bool enabled
        {
            get => this.GetUpdater();
            set => this.SetUpdater(value);
        }

        public Updatable(Action onUpdate = null, bool enabled = false)
        {
            OnUpdate = onUpdate;
            if (enabled)
                this.enabled = true;
        }

        void IUpdatable.OnUpdate()
        {
            OnUpdated();
            OnUpdate?.Invoke();
        }

        protected virtual void OnUpdated()
        {
        }
    }
}