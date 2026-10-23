using System;
using Entities.Blackboards;
using Entities.Car;
using Entities.Zombies;
using UnityEngine;
using UnityEngine.Pool;
using VContainer;

namespace GameUI
{
    // Floating damage numbers over hit zombies and the car, taken from a pool
    public class DamagePopupsUI : MonoBehaviour
    {
        private const string ZOMBIE_DAMAGE_FORMAT = "{0:0}";
        private const string CAR_DAMAGE_FORMAT = "-{0:0}";

        [Inject] private CarComponent car;
        [Inject] private ZombiesBlackboard blackboard;
        [Inject] private Camera mainCamera;

        [SerializeField] private DamagePopupUI popupPrefab;
        [Tooltip("Full-screen area the numbers are placed in")]
        [SerializeField] private RectTransform area;
        [SerializeField] private int poolSize = 16;
        [Tooltip("Random shift in canvas units, so the numbers of rapid hits don't cover each other")]
        [SerializeField] private float spread = 80f;

        [Header("Zombie hit")]
        [SerializeField] private Color zombieDamageColor = new Color(1f, 0.27f, 0.22f);
        [Tooltip("Above the zombie health bar")]
        [SerializeField] private float zombieHeadHeight = 2.5f;

        [Header("Car hit")]
        [SerializeField] private Color carDamageColor = new Color(0.36f, 0.9f, 0.3f);
        [SerializeField] private float carTopHeight = 2.6f;

        private ObjectPool<DamagePopupUI> pool;
        private Action<DamagePopupUI> releasePopup;
        private Camera canvasCamera;

        // Start: the car creates its Health in Awake
        private void Start()
        {
            Canvas canvas = area.GetComponentInParent<Canvas>().rootCanvas;
            canvasCamera = canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : canvas.worldCamera;

            pool = new ObjectPool<DamagePopupUI>(CreatePopup, OnGetPopup, OnReleasePopup, OnDestroyPopup, false, poolSize);
            releasePopup = pool.Release;
            PrewarmPopups();

            car.Health.Damaged += OnHandleCarDamaged;
            blackboard.OnZombieDamaged += OnHandleZombieDamaged;
        }

        private void OnDestroy()
        {
            car.Health.Damaged -= OnHandleCarDamaged;
            blackboard.OnZombieDamaged -= OnHandleZombieDamaged;
        }

        private void OnHandleZombieDamaged(ZombieComponent zombie, float damage)
        {
            Show(zombie.transform, zombieHeadHeight, ZOMBIE_DAMAGE_FORMAT, damage, zombieDamageColor);
        }

        private void OnHandleCarDamaged(float damage)
        {
            Show(car.transform, carTopHeight, CAR_DAMAGE_FORMAT, damage, carDamageColor);
        }

        private void Show(Transform target, float height, string format, float damage, Color color)
        {
            Vector2 spreadOffset = UnityEngine.Random.insideUnitCircle * spread;
            pool.Get().Play(target, Vector3.up * height, spreadOffset, format, damage, color, releasePopup);
        }

        private void PrewarmPopups()
        {
            var popups = new DamagePopupUI[poolSize];
            for (int i = 0; i < popups.Length; i++)
            {
                popups[i] = pool.Get();
            }

            foreach (DamagePopupUI popup in popups)
            {
                pool.Release(popup);
            }
        }

        private DamagePopupUI CreatePopup()
        {
            DamagePopupUI popup = Instantiate(popupPrefab, area);
            popup.Init(mainCamera, canvasCamera, area);
            return popup;
        }

        private static void OnGetPopup(DamagePopupUI popup)
        {
            popup.gameObject.SetActive(true);
        }

        private static void OnReleasePopup(DamagePopupUI popup)
        {
            popup.gameObject.SetActive(false);
        }

        private static void OnDestroyPopup(DamagePopupUI popup)
        {
            Destroy(popup.gameObject);
        }
    }
}
