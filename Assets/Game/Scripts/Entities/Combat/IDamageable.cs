using UnityEngine;

namespace Entities.Combat
{
    public interface IDamageable
    {
        void TakeDamage(float damage, Vector3 direction);
    }
}
