using CoreAudio;
using CoreGameplay.Components;
using CoreGameplay.Components.Interfaces;
using DG.Tweening;
using Game.Scripts.CoreGameplay.Components;
using Memento;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace GameUI
{
    public class TimeReviveUI : MonoBehaviour
    {
        [Inject] private SoundSettings soundSettings;
        [Inject] private MoneyComponent moneyComponent;
        [Inject] private ProgressTrackerComponent progressTrackerComponent;
        [Inject] private LevelManager levelManager;
        [Inject] private ScreenFader screenFader;
        [Inject] private IAnalyticsComponent analyticsComponent;
        [SerializeField] private GameObject panel;
        [SerializeField] private Image heartIcon;
        [SerializeField] private float beatScale = 1.2f;
        [SerializeField] private float beatDuration = 0.3f;
        private Sequence beatSequence;

        private bool isClickedToContinue;

        private void OnEnable()
        {
            progressTrackerComponent.OnPreFailGame += OnHandlePreFailGame;
        }

        private void OnDisable()
        {
            progressTrackerComponent.OnPreFailGame -= OnHandlePreFailGame;
        }

        [Button]
        private void OnHandlePreFailGame()
        {
            panel.SetActive(true);
            beatSequence = DOTween.Sequence()
                .Append(heartIcon.transform.DOScale(beatScale, beatDuration).SetEase(Ease.OutQuad))
                .Append(heartIcon.transform.DOScale(1f, beatDuration).SetEase(Ease.InQuad))
                .SetLoops(-1, LoopType.Restart);
        }

        public void Restart()
        {
            if (soundSettings.clickByAnyButton != null)
            {
                SFXManager.Main.Play(soundSettings.clickByAnyButton);
            }
            beatSequence?.Kill();
            if (isClickedToContinue)
                return;
            isClickedToContinue = true;
            analyticsComponent.SendLevelCompletedEvent(false);
            screenFader.FadeIn((() =>
            {
                levelManager.CallReleasePools();
                levelManager.LoadCurrentLevel();
            }));
        }

        public void Continue()
        {
            beatSequence?.Kill();
            if (soundSettings.clickByAnyButton != null)
            {
                SFXManager.Main.Play(soundSettings.clickByAnyButton);
            }
            isClickedToContinue = true;
            var price = levelManager.GetCurrentLevelItem().priceRevive;
            if (moneyComponent.IsEnoughMoney(price))
            {
                moneyComponent.SubMoney(price);
                moneyComponent.Save();
                MementoSystem.StoreCacheFile();
                isClickedToContinue = false;
                panel.SetActive(false);
            }
        }
    }
}
