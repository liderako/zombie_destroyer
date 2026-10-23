using System;
using TMPro;
using UnityEngine;

namespace GameUI
{
    // Follows its target on screen, floats up and fades out, then returns to the pool
    public class DamagePopupUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text label;
        [SerializeField] private float lifetime = 0.8f;
        [Tooltip("How far the number floats up, in canvas units")]
        [SerializeField] private float riseDistance = 170f;
        [Tooltip("The number pops in from this scale")]
        [SerializeField] private float popScale = 1.6f;
        [SerializeField] private float popDuration = 0.15f;
        [Tooltip("Part of the lifetime after which the number starts to fade")]
        [SerializeField, Range(0f, 1f)] private float fadeStart = 0.4f;

        private Camera worldCamera;
        private Camera canvasCamera;
        private RectTransform area;
        private Transform target;
        private Vector3 targetOffset;
        private Vector3 worldPoint;
        private Vector2 spreadOffset;
        private float elapsed;
        private Action<DamagePopupUI> onFinished;

        public void Init(Camera worldCamera, Camera canvasCamera, RectTransform area)
        {
            this.worldCamera = worldCamera;
            this.canvasCamera = canvasCamera;
            this.area = area;
        }

        public void Play(Transform target, Vector3 targetOffset, Vector2 spreadOffset, string format, float damage, Color color,
            Action<DamagePopupUI> onFinished)
        {
            this.target = target;
            this.targetOffset = targetOffset;
            this.spreadOffset = spreadOffset;
            this.onFinished = onFinished;
            label.SetText(format, damage);
            label.color = color;
            elapsed = 0f;
            UpdateView();
        }

        private void Update()
        {
            elapsed += Time.deltaTime;
            if (elapsed >= lifetime)
            {
                onFinished(this);
                return;
            }

            UpdateView();
        }

        private void UpdateView()
        {
            // A hidden (killed) target: the number stays where it was
            if (target.gameObject.activeInHierarchy)
            {
                worldPoint = target.position + targetOffset;
            }

            Vector3 screenPoint = worldCamera.WorldToScreenPoint(worldPoint);
            label.enabled = screenPoint.z > 0f;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(area, screenPoint, canvasCamera, out Vector2 position);

            float progress = elapsed / lifetime;
            float rise = 1f - (1f - progress) * (1f - progress);
            transform.localPosition = position + spreadOffset + Vector2.up * (riseDistance * rise);

            float pop = Mathf.Clamp01(elapsed / popDuration);
            transform.localScale = Vector3.one * Mathf.Lerp(popScale, 1f, pop * (2f - pop));
            label.alpha = 1f - Mathf.InverseLerp(fadeStart, 1f, progress);
        }
    }
}
