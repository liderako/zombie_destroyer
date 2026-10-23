using System;
using System.Threading;
using CoreGameplay.Components;
using CoreGameplay.Components.Interfaces;
using Game.Scripts.CoreGameplay.Components;
using Cysharp.Threading.Tasks;
using GameUI;
using UnityEngine;
using VContainer;

namespace Entities
{
    public class WinScreenPresenter : MonoBehaviour
    {
        [Inject] private ScreenFader screenFader;
        [Inject] private LevelManager levelManager;
        [Inject] private ProgressTrackerComponent progressTracker;
        [Inject] private IAnalyticsComponent analytics;

        [SerializeField] private WinUI winUI;
        [Tooltip("The car drives on past the finish line this long before the screen fades")]
        [SerializeField] private float fadeDelay = 1f;

        private void OnEnable()
        {
            progressTracker.OnFinish += OnHandleFinish;
            winUI.OnClickButtonContinue += OnHandleContinue;
        }

        private void OnDisable()
        {
            progressTracker.OnFinish -= OnHandleFinish;
            winUI.OnClickButtonContinue -= OnHandleContinue;
        }

        private void OnHandleFinish()
        {
            ShowWinAsync(destroyCancellationToken).Forget();
        }

        // The fader is drawn over the window, so the window is shown while the screen is black
        private async UniTaskVoid ShowWinAsync(CancellationToken token)
        {
            bool isCanceled = await UniTask.Delay(TimeSpan.FromSeconds(fadeDelay), cancellationToken: token)
                .SuppressCancellationThrow();
            if (isCanceled)
            {
                return;
            }

            screenFader.FadeIn(() =>
            {
                if (token.IsCancellationRequested)
                {
                    return;
                }

                winUI.Show();
                screenFader.FadeOut();
            });
        }

        private void OnHandleContinue()
        {
            analytics.SendLevelCompletedEvent(true);
            screenFader.FadeIn(() =>
            {
                levelManager.CallReleasePools();
                levelManager.LoadNextLevel();
            });
        }
    }
}
