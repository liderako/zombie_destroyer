using System;
using System.Collections.Generic;
using Entities.Zombies;
using Game.Scripts.CoreGameplay.Components;
using UnityEngine;
using VContainer;

namespace Entities.Blackboards
{
    public class ZombiesBlackboard : MonoBehaviour
    {
        [Inject] private ProgressTrackerComponent progressTracker;

        private readonly List<ZombieComponent> zombies = new List<ZombieComponent>();

        public int KilledCount { get; private set; }

        public event Action<int> OnZombieKilled;
        public event Action<ZombieComponent, float> OnZombieDamaged;

        private void OnEnable()
        {
            progressTracker.OnFinish += FreezeAll;
            progressTracker.OnFailGame += FreezeAll;
        }

        private void OnDisable()
        {
            progressTracker.OnFinish -= FreezeAll;
            progressTracker.OnFailGame -= FreezeAll;
        }

        public void AddZombie(ZombieComponent zombie)
        {
            zombies.Add(zombie);
        }

        public void NotifyDamaged(ZombieComponent zombie, float damage)
        {
            OnZombieDamaged?.Invoke(zombie, damage);
        }

        public void NotifyKilled(ZombieComponent zombie)
        {
            zombies.Remove(zombie);
            KilledCount++;
            OnZombieKilled?.Invoke(KilledCount);
        }

        private void FreezeAll()
        {
            foreach (ZombieComponent zombie in zombies)
            {
                zombie.Freeze();
            }
        }
    }
}
