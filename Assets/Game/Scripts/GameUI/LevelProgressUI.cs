using Game.Scripts.CoreGameplay.Components;
using UnityEngine;
using VContainer;

namespace GameUI
{
    public class LevelProgressUI : MonoBehaviour
    {
        [Inject] private ProgressTrackerComponent progressTracker;

        [Tooltip("Sliced fill: its width follows the progress, so the rounded ends keep their shape")]
        [SerializeField] private RectTransform fill;

        private void Start()
        {
            SetProgress(0f);
        }

        private void OnEnable()
        {
            progressTracker.OnUpdateProgress += OnHandleUpdateProgress;
        }

        private void OnDisable()
        {
            progressTracker.OnUpdateProgress -= OnHandleUpdateProgress;
        }

        private void OnHandleUpdateProgress(int current, int max)
        {
            SetProgress((float)current / max);
        }

        private void SetProgress(float progress)
        {
            fill.gameObject.SetActive(progress > 0f);
            fill.anchorMax = new Vector2(progress, fill.anchorMax.y);
        }
    }
}
