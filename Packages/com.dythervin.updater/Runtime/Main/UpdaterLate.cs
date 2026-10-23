using System;
using Dythervin.Collections;
using Dythervin.Core;

namespace Dythervin.UpdateSystem.Main
{
    public sealed class UpdaterLate : UpdaterBase<UpdaterLate, IUpdatableLate, IUpdatableLateDelta>
    {
        protected override void Update(LockableHashSet<IUpdatableLateDelta> updatables, float deltaTime)
        {
            foreach (IUpdatableLateDelta updatable in updatables)
            {
                try
                {
                    updatable.OnUpdate(deltaTime);
                }
                catch (Exception e)
                {
                    DDebug.LogError(e);
                }
            }
        }

        protected override void Update(LockableHashSet<IUpdatableLate> updatables, float deltaTime)
        {
            foreach (IUpdatableLate updatable in updatables)
            {
                try
                {
                    updatable.OnUpdate();
                }
                catch (Exception e)
                {
                    DDebug.LogError(e);
                }
            }
        }
    }
}