using DG.Tweening;
using Game.Scripts.CoreGameplay.Components;
using UnityEngine;
using VContainer;

namespace GameUI
{
    // Keeps gameplay widgets hidden on the start screen and pops them in when the level starts
    [RequireComponent(typeof(CanvasGroup))]
    public class ShowOnStartUI : MonoBehaviour
    {
        [Inject] private ProgressTrackerComponent progressTracker;

        [SerializeField] private CanvasGroup canvasGroup;
        [SerializeField] private float showDuration = 0.3f;
        [SerializeField] private float startScale = 0.8f;

        private void Awake()
        {
            canvasGroup.alpha = 0f;
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
            canvasGroup.DOFade(1f, showDuration).SetLink(gameObject);
            transform.localScale = Vector3.one * startScale;
            transform.DOScale(1f, showDuration).SetEase(Ease.OutBack).SetLink(gameObject);
        }
    }
}
