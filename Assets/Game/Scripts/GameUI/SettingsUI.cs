using CoreAudio;
using CoreGameplay.Components;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using GeneralUI.Components;
using Haptic.Components;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace GameUI
{
    public class SettingsUI : BaseCanvasUI
    {
        [Inject] private LevelManager levelManager;
        [Inject] private HapticComponent hapticComponent;
        [Inject] private AudioGameService audioGameService;
        [Inject] private ScreenFader screenFader;
        [Inject] private SoundSettings soundSettings;
        
        [Header("References")] 
        [SerializeField] private FadeUI background;
        [SerializeField] private RectTransform content;
        [SerializeField] private RectTransform popup;
        [SerializeField] private Button closeBtn;
        [SerializeField] private Button resumeBtn;
        [SerializeField] private Button restartBtn;
        [SerializeField] private Toggle soundToggle;
        [SerializeField] private Toggle hapticToggle;
        [SerializeField] private Toggle songToggle;

        [Header("Appear animation")] 
        [SerializeField] private float duration;
        [SerializeField] private Ease showEase = Ease.OutBack;
        [SerializeField] private Ease hideEase = Ease.InBack;
        
        private Tween tween;

        private void Start()
        {
            soundToggle.isOn = audioGameService.IsSoundEnable;
            songToggle.isOn = audioGameService.IsMusicEnable;
            hapticToggle.isOn = hapticComponent.VibrationAvailable;

            closeBtn.onClick.AddListener(Hide);
            resumeBtn.onClick.AddListener(Hide);
            restartBtn.onClick.AddListener(Restart);
            soundToggle.onValueChanged.AddListener(SoundChanged);
            hapticToggle.onValueChanged.AddListener(HapticChanged);
            songToggle.onValueChanged.AddListener(SongChanged);
        }

        public override void Show()
        {
            base.Show();
            if (soundSettings.clickByAnyButton != null)
            {
                SFXManager.Main.Play(soundSettings.clickByAnyButton);
            }
            ShowAsync().Forget();
        }

        public override void Hide()
        {
            if (soundSettings.clickByAnyButton != null)
            {
                SFXManager.Main.Play(soundSettings.clickByAnyButton);
            }
            HideAsync().Forget();
        }

        private async UniTask ShowAsync()
        {
            content.gameObject.SetActive(true);
                
            background.Show();
            
            tween?.Kill();
            tween = popup.DOScale(1, duration).From(0).SetEase(showEase).SetLink(gameObject);
            
            await tween.AsyncWaitForCompletion();
        }

        private async UniTask HideAsync()
        {
            background.Rewind();
            
            tween?.Kill();
            tween = popup.DOScale(0, duration).From(1).SetEase(hideEase).SetLink(gameObject);
            await tween.AsyncWaitForCompletion();
            
            content.gameObject.SetActive(false);
            base.Hide();
        }

        private void SoundChanged(bool value)
        {
            audioGameService.ChangeSoundStatus(value);
            if (soundSettings.clickByAnyButton != null)
            {
                SFXManager.Main.Play(soundSettings.clickByAnyButton);
            }
        }

        private void HapticChanged(bool value)
        {
            hapticComponent.VibrationAvailable = value;
            if (value)
            {
                hapticComponent.LightImpact();
            }
            if (soundSettings.clickByAnyButton != null)
            {
                SFXManager.Main.Play(soundSettings.clickByAnyButton);
            }
        }
        
        private void SongChanged(bool value)
        {
            audioGameService.ChangeMusicStatus(value);
            if (soundSettings.clickByAnyButton != null)
            {
                SFXManager.Main.Play(soundSettings.clickByAnyButton);
            }
        }

        private void Restart()
        {
            if (soundSettings.clickByAnyButton != null)
            {
                SFXManager.Main.Play(soundSettings.clickByAnyButton);
            }
            screenFader.FadeIn((() =>
            {
                levelManager.CallReleasePools();
                levelManager.LoadCurrentLevel();
            }));
        }
    }
}