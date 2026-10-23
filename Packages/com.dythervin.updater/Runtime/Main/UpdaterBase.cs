using System;
using System.Collections.Generic;
using Dythervin.Collections;
using Dythervin.Core;
using Dythervin.ObjectPool;
#if ODIN_INSPECTOR && UNITY_EDITOR
using Sirenix.OdinInspector;
#endif

namespace Dythervin.UpdateSystem.Main
{
    public abstract class UpdaterBase<TUpdater, TElement> : Singleton<TUpdater>, IUpdater
        where TUpdater : UpdaterBase<TUpdater, TElement>, new()
    {
#if ODIN_INSPECTOR && UNITY_EDITOR
        [ShowInInspector, ReadOnly]
#endif
        private readonly LockableHashSet<TElement> _updatables = new LockableHashSet<TElement>();

        private readonly List<Action> _onNextUpdate = new List<Action>();

        public void OnNextUpdate(Action action)
        {
            if (action == null)
                return;

            _onNextUpdate.Add(action);
        }

        public virtual void Set(TElement element, bool value)
        {
            if (value)
                _updatables.Add(element);
            else
                _updatables.Remove(element);
        }

        internal bool Contains(TElement listener)
        {
            return _updatables.Contains(listener);
        }

        protected abstract void Update(LockableHashSet<TElement> updatables, float deltaTime);

        public virtual void Update(float deltaTime)
        {
            _updatables.SetLock(true);
            Update(_updatables, deltaTime);
            _updatables.SetLock(false);

            UpdateOnce();
        }

        private void UpdateOnce()
        {
            if (_onNextUpdate.Count == 0)
                return;

            foreach (Action action in _onNextUpdate.ToTempEnumerableArray())
            {
                action.Invoke();
            }

            _onNextUpdate.Clear();
        }
    }

    public abstract class UpdaterBase<TUpdater, TElement, TElementDelta> : UpdaterBase<TUpdater, TElement>
        where TUpdater : UpdaterBase<TUpdater, TElement, TElementDelta>, new()
    {
#if ODIN_INSPECTOR && UNITY_EDITOR
        [ShowInInspector, ReadOnly]
#endif
        private readonly LockableHashSet<TElementDelta> _valuesDelta = new LockableHashSet<TElementDelta>();

        public virtual void Set(TElementDelta element, bool value)
        {
            if (value)
                _valuesDelta.Add(element);
            else
                _valuesDelta.Remove(element);
        }

        internal bool Contains(TElementDelta listener)
        {
            return _valuesDelta.Contains(listener);
        }

        protected abstract void Update(LockableHashSet<TElementDelta> updatables, float deltaTime);

        public override void Update(float deltaTime)
        {
            base.Update(deltaTime);
            _valuesDelta.SetLock(true);
            Update(_valuesDelta, deltaTime);
            _valuesDelta.SetLock(false);
        }
    }
}