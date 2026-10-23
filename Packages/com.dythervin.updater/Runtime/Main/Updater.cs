using System;
using Dythervin.Collections;
using Dythervin.Core;

namespace Dythervin.UpdateSystem.Main
{
    public sealed class Updater : UpdaterBase<Updater, IUpdatable, IUpdatableDelta>
    {
        protected override void Update(LockableHashSet<IUpdatableDelta> updatables, float deltaTime)
        {
            foreach (IUpdatableDelta updatable in updatables)
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

        protected override void Update(LockableHashSet<IUpdatable> updatables, float deltaTime)
        {
            foreach (IUpdatable updatable in updatables)
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