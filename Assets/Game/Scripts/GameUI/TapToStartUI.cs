using DG.Tweening;
using Game.Scripts.CoreGameplay.Components;
using UnityEngine;
using VContainer;

namespace GameUI
{
    [RequireComponent(typeof(CanvasGroup))]
    public class TapToStartUI : MonoBehaviour
    {
        [Inject] private ProgressTrackerComponent progressTracker;

        [SerializeField] private CanvasGroup canvasGroup;
        [SerializeField] private Transform pulseTarget;
        [SerializeField] private float pulseScale = 1.08f;
        [SerializeField] private float pulseDuration = 0.6f;
        [SerializeField] private float hideDuration = 0.2f;

        private void Start()
        {
            pulseTarget.DOScale(pulseScale, pulseDuration)
                .SetEase(Ease.InOutSine)
                .SetLoops(-1, LoopType.Yoyo)
                .SetLink(gameObject);
        }

        private void OnEnable()
        {
            progressTracker.OnStartGame += OnHandleStartGame;
        }

        private void OnDisable()
        {
            progressTracker.OnStartGame -= OnHandleStartGame;
        }

        private void OnHandleStartGame()
        {
            canvasGroup.DOFade(0f, hideDuration)
                .SetLink(gameObject)
                .OnComplete(() => gameObject.SetActive(false));
        }
    }
}
