using Entities.Zombies;
using UnityEngine;
using VContainer;

namespace GameUI
{
    // Shown after the first hit; faces the camera and fades out with the fog
    public class ZombieHealthBarUI : MonoBehaviour
    {
        private const float VISIBILITY_STEP = 0.01f;

        [Inject] private Camera mainCamera;

        [SerializeField] private ZombieComponent zombie;
        [Tooltip("Sliced sprites: the fill width follows the health, so the rounded ends keep their shape")]
        [SerializeField] private SpriteRenderer background;
        [SerializeField] private SpriteRenderer fill;

        private Color backgroundColor;
        private Color fillColor;
        private float fullWidth;
        private float visibility = -1f;

        private void Start()
        {
            backgroundColor = background.color;
            fillColor = fill.color;
            fullWidth = fill.size.x;
            SetShown(false);

            zombie.Health.Changed += OnHandleHealthChanged;
            zombie.Health.Died += OnHandleDied;
        }

        private void OnDestroy()
        {
            zombie.Health.Changed -= OnHandleHealthChanged;
            zombie.Health.Died -= OnHandleDied;
        }

        private void LateUpdate()
        {
            if (!fill.enabled)
            {
                return;
            }

            Transform view = mainCamera.transform;
            transform.rotation = view.rotation;

            float distance = Vector3.Distance(view.position, transform.position);
            float fogVisibility = 1f - Mathf.InverseLerp(RenderSettings.fogStartDistance, RenderSettings.fogEndDistance, distance);
            if (Mathf.Abs(fogVisibility - visibility) < VISIBILITY_STEP)
            {
                return;
            }

            visibility = fogVisibility;
            background.color = WithAlpha(backgroundColor, backgroundColor.a * visibility);
            fill.color = WithAlpha(fillColor, fillColor.a * visibility);
        }

        private void OnHandleHealthChanged()
        {
            SetShown(true);

            // A sliced sprite can't be narrower than its rounded ends
            float height = fill.size.y;
            float width = Mathf.Max(fullWidth * zombie.Health.Normalized, height);
            fill.size = new Vector2(width, height);
            fill.transform.localPosition = new Vector3((width - fullWidth) * 0.5f, 0f, 0f);
        }

        private void OnHandleDied()
        {
            gameObject.SetActive(false);
        }

        private void SetShown(bool isShown)
        {
            background.enabled = isShown;
            fill.enabled = isShown;
        }

        private static Color WithAlpha(Color color, float alpha)
        {
            color.a = alpha;
            return color;
        }
    }
}
