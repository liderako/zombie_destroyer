using DG.Tweening;
using Entities.Car;
using TMPro;
using UnityEngine;
using VContainer;

namespace GameUI
{
    // Vertical bar: the fill grows from the bottom
    public class HealthBarUI : MonoBehaviour
    {
        [Inject] private CarComponent car;

        [Tooltip("Sliced fill: its height follows the health, so the rounded ends keep their shape")]
        [SerializeField] private RectTransform fill;
        [Tooltip("Current health number on the heart")]
        [SerializeField] private TMP_Text valueLabel;
        [SerializeField] private float fillDuration = 0.25f;
        [SerializeField] private Vector3 hitPunch = new Vector3(0.08f, 0.08f, 0f);

        private void Start()
        {
            fill.anchorMax = new Vector2(fill.anchorMax.x, car.Health.Normalized);
            ShowValue();
            car.Health.Changed += OnHandleHealthChanged;
        }

        private void OnDestroy()
        {
            car.Health.Changed -= OnHandleHealthChanged;
        }

        private void OnHandleHealthChanged()
        {
            ShowValue();

            fill.DOKill();
            fill.DOAnchorMax(new Vector2(fill.anchorMax.x, car.Health.Normalized), fillDuration);

            transform.DOKill(true);
            transform.DOPunchScale(hitPunch, fillDuration);
        }

        private void ShowValue()
        {
            // Rounded up: a car with 0.4 HP left is still alive
            valueLabel.SetText("{0:0}", Mathf.Ceil(car.Health.Current));
        }
    }
}
