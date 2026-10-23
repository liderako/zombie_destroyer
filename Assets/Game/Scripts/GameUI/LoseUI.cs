using CoreAudio;
using CoreGameplay.Components;
using CoreGameplay.Components.Interfaces;
using Game.Scripts.CoreGameplay.Components;
using UnityEngine;
using VContainer;

namespace GameUI
{
    public class LoseUI : MonoBehaviour
    {
        [Inject] private SoundSettings soundSettings;
        [Inject] private ProgressTrackerComponent progressTrackerComponent;
        [Inject] private LevelManager levelManager;
        [Inject] private ScreenFader screenFader;
        [Inject] private IAnalyticsComponent analyticsComponent;

        [SerializeField] private GameObject panel;
        private bool isClickedToContinue;

        private void OnEnable()
        {
            progressTrackerComponent.OnFailGame += Show;
        }

        private void OnDisable()
        {
            progressTrackerComponent.OnFailGame -= Show;
        }

        public void Show()
        {
            if (soundSettings.loseScreenAppear != null)
            {
                SFXManager.Main.Play(soundSettings.loseScreenAppear);
            }
            panel.SetActive(true);
        }

        public void Restart()
        {
            if (isClickedToContinue)
                return;

            if (soundSettings.clickByAnyButton != null)
            {
                SFXManager.Main.Play(soundSettings.clickByAnyButton);
            }
            analyticsComponent.SendLevelCompletedEvent(false);
            isClickedToContinue = true;
            screenFader.FadeIn((() =>
            {
                levelManager.CallReleasePools();
                levelManager.LoadCurrentLevel();
            }));
        }
    }
}
