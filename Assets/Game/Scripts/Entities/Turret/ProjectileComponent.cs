using System;
using Entities.Combat;
using HcCoreConfigs.Settings;
using UnityEngine;

namespace Entities.Turret
{
    public class ProjectileComponent : MonoBehaviour
    {
        [SerializeField] private TrailRenderer trail;

        private TurretSettings settings;
        private Action<ProjectileComponent> onFinished;
        private Vector3 velocity;
        private Vector3 castPosition;
        private float lifeTime;

        /// <param name="castOrigin">Start of the first hit check. It is the turret axis, because the muzzle can be inside a zombie</param>
        public void Launch(Vector3 castOrigin, Vector3 position, Vector3 direction, TurretSettings settings, Action<ProjectileComponent> onFinished)
        {
            this.settings = settings;
            this.onFinished = onFinished;
            velocity = direction * settings.projectileSpeed;
            castPosition = castOrigin;
            lifeTime = settings.projectileLifetime;

            // Position first, then clear: otherwise the trail stretches from the previous shot
            transform.SetPositionAndRotation(position, Quaternion.LookRotation(direction));
            trail.Clear();
            trail.emitting = true;
        }

        private void Update()
        {
            Vector3 nextPosition = transform.position + velocity * Time.deltaTime;
            Vector3 segment = nextPosition - castPosition;

            if (Physics.SphereCast(castPosition, settings.projectileRadius, segment.normalized, out RaycastHit hit,
                    segment.magnitude, settings.hitMask, QueryTriggerInteraction.Ignore))
            {
                if (hit.collider.TryGetComponent(out IDamageable damageable))
                {
                    damageable.TakeDamage(settings.damage, velocity.normalized);
                }

                transform.position = hit.point;
                Finish();
                return;
            }

            transform.position = nextPosition;
            castPosition = nextPosition;

            lifeTime -= Time.deltaTime;
            if (lifeTime <= 0f)
            {
                Finish();
            }
        }

        private void Finish()
        {
            trail.emitting = false;
            onFinished.Invoke(this);
        }
    }
}
