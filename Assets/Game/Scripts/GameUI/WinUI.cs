using System;
using Components.CoreGameplay.Behavior;
using CoreAudio;
using CoreGameplay.Components;
using DG.Tweening;
using GeneralUI.Components;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace GameUI
{
    public class WinUI : BaseCanvasUI
    {
        [Inject] private SoundSettings soundSettings;
        [Inject] private LevelManager levelManager;

        [Header("General Components")]
        [SerializeField] private Button buttonContinue;
        [SerializeField] private GameObject header;
        [SerializeField] private CounterUI rewardUi;

        [Header("Coins Components")]
        [Inject] private SpriteFlyBehaviorComponent spriteFlyBehaviorComponent;
        [SerializeField] private CoinBarUI coinBarUI;

        private int rewardAmount;
        private bool isClickedToContinue;

        public Action OnClickButtonContinue;

        public override void Show()
        {
            base.Show();

            if (soundSettings.winScreenAppear != null)
            {
                SFXManager.Main.Play(soundSettings.winScreenAppear);
            }

            header.gameObject.SetActive(false);
            DOVirtual.DelayedCall(0.35f, () => header.gameObject.SetActive(true));
            var seq = DOTween.Sequence();
            seq.AppendInterval(0.6f);
            seq.OnComplete(() =>
            {
                rewardAmount = levelManager.GetCurrentLevelItem().GetRewardForLevel();
                rewardUi.LerpPlay(0, rewardAmount);
                spriteFlyBehaviorComponent.AddFlyAnimation(
                    rewardAmount,
                    rewardUi,
                    coinBarUI.CounterBar,
                30,
                null);
            });
        }

        public void ContinueClick()
        {
            if (isClickedToContinue)
                return;
            isClickedToContinue = true;
            coinBarUI.ResetPool();
            OnClickButtonContinue?.Invoke();
            if (soundSettings.clickByAnyButton != null)
            {
                SFXManager.Main.Play(soundSettings.clickByAnyButton);
            }
        }
    }
}
