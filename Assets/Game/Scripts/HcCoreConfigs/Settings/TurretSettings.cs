using UnityEngine;

namespace HcCoreConfigs.Settings
{
    [CreateAssetMenu(fileName = "TurretSettings", menuName = "ScriptableObjects/Settings/TurretSettings", order = 0)]
    public class TurretSettings : ScriptableObject
    {
        [Header("Aim")]
        public float aimSmoothTime = 0.06f;

        [Header("Fire")]
        public float fireRate = 7f;
        public float recoilDistance = 0.12f;
        public float recoilRecoverSpeed = 1.2f;

        [Header("Projectile")]
        public float damage = 25f;
        public float projectileSpeed = 45f;
        [Tooltip("Lifetime × speed is the flight distance; the muzzle pitch makes bullets reach the ground at ~160 m")]
        public float projectileLifetime = 3.7f;
        public float projectileRadius = 0.4f;
        public LayerMask hitMask;
        public int projectilePoolSize = 32;
    }
}
