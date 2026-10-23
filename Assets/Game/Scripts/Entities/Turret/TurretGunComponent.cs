using System;
using Game.Scripts.CoreGameplay.Components;
using HcCoreConfigs.Settings;
using UnityEngine;
using UnityEngine.Pool;
using VContainer;

namespace Entities.Turret
{
    public class TurretGunComponent : MonoBehaviour
    {
        [Inject] private ProgressTrackerComponent progressTracker;

        [SerializeField] private TurretSettings settings;
        [SerializeField] private Transform castOrigin;
        [SerializeField] private Transform muzzle;
        [SerializeField] private Transform recoilTarget;
        [SerializeField] private ParticleSystem muzzleFlash;

        [Header("Projectiles")]
        [SerializeField] private ProjectileComponent projectilePrefab;
        [SerializeField] private Transform projectilesRoot;

        private ObjectPool<ProjectileComponent> projectilePool;
        private Action<ProjectileComponent> releaseProjectile;
        private Vector3 recoilRestPosition;
        private float recoilOffset;
        private float cooldown;
        private bool isFiring;

        private void Awake()
        {
            projectilePool = new ObjectPool<ProjectileComponent>(CreateProjectile, OnGetProjectile, OnReleaseProjectile,
                OnDestroyProjectile, false, settings.projectilePoolSize);
            releaseProjectile = projectilePool.Release;
            recoilRestPosition = recoilTarget.localPosition;
            PrewarmProjectiles();
        }

        private void OnEnable()
        {
            progressTracker.OnStartGame += OnHandleStartGame;
            progressTracker.OnFinish += OnHandleStopGame;
            progressTracker.OnFailGame += OnHandleStopGame;
        }

        private void OnDisable()
        {
            progressTracker.OnStartGame -= OnHandleStartGame;
            progressTracker.OnFinish -= OnHandleStopGame;
            progressTracker.OnFailGame -= OnHandleStopGame;
        }

        private void Update()
        {
            recoilOffset = Mathf.MoveTowards(recoilOffset, 0f, settings.recoilRecoverSpeed * Time.deltaTime);
            recoilTarget.localPosition = recoilRestPosition + Vector3.back * recoilOffset;

            if (!isFiring)
            {
                return;
            }

            cooldown -= Time.deltaTime;
            if (cooldown <= 0f)
            {
                cooldown = 1f / settings.fireRate;
                Fire();
            }
        }

        private void Fire()
        {
            ProjectileComponent projectile = projectilePool.Get();
            projectile.Launch(castOrigin.position, muzzle.position, muzzle.forward, settings, releaseProjectile);
            muzzleFlash.Play();
            recoilOffset = settings.recoilDistance;
        }

        private void OnHandleStartGame()
        {
            isFiring = true;
        }

        private void OnHandleStopGame()
        {
            isFiring = false;
        }

        private void PrewarmProjectiles()
        {
            var projectiles = new ProjectileComponent[settings.projectilePoolSize];
            for (int i = 0; i < projectiles.Length; i++)
            {
                projectiles[i] = projectilePool.Get();
            }

            foreach (ProjectileComponent projectile in projectiles)
            {
                projectilePool.Release(projectile);
            }
        }

        private ProjectileComponent CreateProjectile()
        {
            return Instantiate(projectilePrefab, projectilesRoot);
        }

        private static void OnGetProjectile(ProjectileComponent projectile)
        {
            projectile.gameObject.SetActive(true);
        }

        private static void OnReleaseProjectile(ProjectileComponent projectile)
        {
            projectile.gameObject.SetActive(false);
        }

        private static void OnDestroyProjectile(ProjectileComponent projectile)
        {
            Destroy(projectile.gameObject);
        }
    }
}
