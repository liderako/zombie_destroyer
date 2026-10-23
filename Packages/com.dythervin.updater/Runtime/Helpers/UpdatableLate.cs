using System;

namespace Dythervin.UpdateSystem.Helpers
{
    public class UpdatableLate : IUpdatableLate, IUpdatableListener
    {
        public event Action OnUpdate;

        public bool enabled
        {
            get => this.GetLateUpdater();
            set => this.SetLateUpdater(value);
        }

        public UpdatableLate(Action onUpdate = null, bool enabled = false)
        {
            OnUpdate = onUpdate;
            if (enabled)
                this.enabled = true;
        }

        void IUpdatableLate.OnUpdate()
        {
            OnUpdate?.Invoke();
        }
    }
}