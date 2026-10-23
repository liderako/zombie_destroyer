using System;
using Dythervin.Collections;
using Dythervin.Core;

namespace Dythervin.UpdateSystem.Main
{
    public sealed class UpdaterInterval : UpdaterBase<UpdaterInterval, IUpdatableInterval>
    {
        public override void Set(IUpdatableInterval element, bool value)
        {
            base.Set(element, value);
            if (value)
                element.UntilUpdate = element.Interval;
        }

        protected override void Update(LockableHashSet<IUpdatableInterval> updatables, float deltaTime)
        {
            foreach (IUpdatableInterval updatable in updatables)
            {
                try
                {
                    if ((updatable.UntilUpdate -= deltaTime) <= 0)
                    {
                        updatable.UntilUpdate = updatable.Interval;
                        updatable.OnUpdate();
                    }
                }
                catch (Exception e)
                {
                    DDebug.LogError(e);
                }
            }
        }
    }
}