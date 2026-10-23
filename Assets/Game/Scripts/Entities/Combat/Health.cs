using System;
using UnityEngine;

namespace Entities.Combat
{
    public class Health
    {
        public float Max { get; }
        public float Current { get; private set; }
        public float Normalized => Current / Max;
        public bool IsDead => Current <= 0f;

        // Damage actually taken, never more than the health left
        public event Action<float> Damaged;
        public event Action Changed;
        public event Action Died;

        public Health(float max)
        {
            Max = max;
            Current = max;
        }

        public void TakeDamage(float damage)
        {
            if (IsDead || damage <= 0f)
            {
                return;
            }

            float previous = Current;
            Current = Mathf.Max(0f, Current - damage);
            Damaged?.Invoke(previous - Current);
            Changed?.Invoke();

            if (IsDead)
            {
                Died?.Invoke();
            }
        }
    }
}
