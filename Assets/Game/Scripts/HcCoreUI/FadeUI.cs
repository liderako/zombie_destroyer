using System;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace GeneralUI.Components
{
    [RequireComponent(typeof(CanvasGroup))]

    public class FadeUI : MonoBehaviour, IViewUI
    {
        public float Delay => delay;
        public float Duration => duration;
        public bool IgnoreDuration => ignoreDuration;

        [BoxGroup("General"), SerializeField] private float delay;
        [BoxGroup("General"), SerializeField] private bool ignoreDuration;

        [BoxGroup("Property"), SerializeField] private Ease easeCurve = Ease.Linear;

        [BoxGroup("Property"), SerializeField, Min(0.1f), Range(0.1f, 5f)]
        private float duration = 1;

        private CanvasGroup canvasGroup;

        private Sequence sequence;

        public void Show()
        {
            DoFade(0, 1);
        }

        public void Rewind()
        {
            DoFade(1, 0);
        }

        public void Rewind(float overrideDuration)
        {
            DoFade(1, 0, overrideDuration);
        }

        public void Hide()
        {
            if (sequence != null)
            {
                sequence.Kill();
                DOTween.Kill(sequence);
            }
        }

        public void SetDuration(float duration) =>
            duration = duration;

        private void DoFade(float start, float end, float duration = 0)
        {
            if (canvasGroup == null)
                canvasGroup = GetComponent<CanvasGroup>();

            if (duration == default)
                duration = duration;

            canvasGroup.alpha = start;
            float value = start;

            // Killed together with the object on a scene reload
            sequence = DOTween.Sequence().SetLink(gameObject);
            sequence.SetEase(easeCurve);

            sequence.AppendInterval(delay);
            sequence.Append(DOTween.To(() => value, x => value = x, end, duration * Time.unscaledDeltaTime)
                .OnUpdate(() => { canvasGroup.alpha = value; }));
        }
    }
}
