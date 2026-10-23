using Entities.Combat;
using Game.Scripts.CoreGameplay.Components;
using HcCoreConfigs.Settings;
using Unity.Cinemachine;
using UnityEngine;
using VContainer;

namespace Entities.Car
{
    public class CarComponent : MonoBehaviour, IDamageable
    {
        [Inject] private ProgressTrackerComponent progressTracker;

        [SerializeField] private CarSettings settings;
        [SerializeField] private CarMovementComponent movement;
        [SerializeField] private CinemachineImpulseSource impulseSource;
        [Tooltip("Body box in local space: zombies measure their reach to its surface, not to the car pivot")]
        [SerializeField] private Bounds bodyBounds = new Bounds(new Vector3(0f, 0.95f, 0f), new Vector3(1.9f, 1.4f, 4.25f));

        private float lastShakeTime = float.NegativeInfinity;

        public Health Health { get; private set; }
        public CarMovementComponent Movement => movement;
        public Bounds BodyBounds => bodyBounds;

        private void Awake()
        {
            Health = new Health(settings.maxHealth);
        }

        private void OnEnable()
        {
            Health.Died += OnHandleDied;
        }

        private void OnDisable()
        {
            Health.Died -= OnHandleDied;
        }

        public void TakeDamage(float damage, Vector3 direction)
        {
            Health.TakeDamage(damage);

            if (Time.time - lastShakeTime >= settings.hitShakeCooldown)
            {
                lastShakeTime = Time.time;
                impulseSource.GenerateImpulseWithVelocity(direction * settings.hitShakeForce);
            }
        }

        public Vector3 ClosestPointOnBody(Vector3 point)
        {
            Vector3 localPoint = transform.InverseTransformPoint(point);
            return transform.TransformPoint(bodyBounds.ClosestPoint(localPoint));
        }

        private void OnHandleDied()
        {
            progressTracker.CallFailGameEvent();
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.matrix = transform.localToWorldMatrix;
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube(bodyBounds.center, bodyBounds.size);
        }
    }
}
