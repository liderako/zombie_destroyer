using UnityEngine;

namespace HcCoreConfigs.Settings
{
    [CreateAssetMenu(fileName = "ZombieSettings", menuName = "ScriptableObjects/Settings/ZombieSettings", order = 0)]
    public class ZombieSettings : ScriptableObject
    {
        [Header("Health")]
        public float maxHealth = 50f;

        [Header("Movement")]
        public float runSpeed = 6.98f;
        [Tooltip("Per-zombie random speed: runSpeed × (1 ± spread)")]
        [Range(0f, 0.5f)] public float runSpeedSpread = 0.2f;
        public float turnSpeed = 540f;
        public float maxLeadTime = 1.5f;
        [Tooltip("Distance to the car body where the zombie stops leading the car and runs straight to it")]
        public float closeDistance = 4f;
        [Tooltip("Random offset of each zombie's approach point along the car's side")]
        public float approachSpread = 2.5f;
        [Tooltip("Sideways wobble while running")]
        public float runWobble = 1.2f;

        [Header("Wander")]
        [Tooltip("Half-width of the wandering area around the spawn point")]
        public float wanderRadius = 2.5f;
        public float walkSpeed = 0.9f;
        public float walkTurnSpeed = 220f;
        [Tooltip("Random pause at each wander point, seconds")]
        public Vector2 wanderIdleTime = new Vector2(2f, 5f);

        [Header("Attack")]
        public float aggroRadius = 26.4f;
        [Tooltip("Distance from the car body surface")]
        public float attackReach = 0.6f;
        [Tooltip("Rearmost attack point along the car (local Z). Zombies behind it run up to the side, " +
                 "so the turret never has to shoot towards the camera")]
        public float rearAttackLimit = -0.6f;
        public float attackDamage = 6f;
        public float attackInterval = 1f;

        [Header("Death")]
        public float deathImpulse = 6f;
        public float corpseLifetime = 1.5f;
        public string corpseLayer = "Corpse";
    }
}
