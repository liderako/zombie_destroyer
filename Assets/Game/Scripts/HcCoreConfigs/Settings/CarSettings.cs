using UnityEngine;

namespace HcCoreConfigs.Settings
{
    [CreateAssetMenu(fileName = "CarSettings", menuName = "ScriptableObjects/Settings/CarSettings", order = 0)]
    public class CarSettings : ScriptableObject
    {
        [Header("Health")]
        public float maxHealth = 100f;

        [Header("Movement")]
        public float maxSpeed = 13f;
        public float acceleration = 8f;
        public float deceleration = 6f;
        [Tooltip("The car crosses the finish line at full speed and brakes only this far past it")]
        public float finishRunOut = 25f;

        [Header("Weave")]
        [Tooltip("How far the car drifts to each side of the road axis")]
        public float weaveAmplitude = 0.6f;
        [Tooltip("Road distance of one full left-right swing")]
        public float weaveWavelength = 60f;

        [Header("Hit Feedback")]
        public float hitShakeForce = 0.4f;
        public float hitShakeCooldown = 0.25f;
    }
}
