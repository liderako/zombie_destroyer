using System;
using Dythervin.Collections;
using Dythervin.Core;

namespace Dythervin.UpdateSystem.Main
{
    public sealed class UpdaterFixed : UpdaterBase<UpdaterFixed, IUpdatableFixed, IUpdatableFixedDelta>
    {
        protected override void Update(LockableHashSet<IUpdatableFixedDelta> updatables, float deltaTime)
        {
            foreach (IUpdatableFixedDelta updatable in updatables)
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

        protected override void Update(LockableHashSet<IUpdatableFixed> updatables, float deltaTime)
        {
            foreach (IUpdatableFixed updatable in updatables)
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