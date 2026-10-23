using System;
using System.Linq;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace GeneralUI.Components
{
    public class ScaleUI : MonoBehaviour, IViewUI
    {
        public float Delay => delay;
        public float Duration => duration;
        public bool IgnoreDuration => ignoreDuration;

        [BoxGroup("General"), SerializeField] private float delay;
        [BoxGroup("General"), SerializeField] private bool ignoreDuration;

        [BoxGroup("Property"), SerializeField] private Ease easeCurve = Ease.Linear;

        [BoxGroup("Property"), SerializeField, Min(0.1f), Range(0.1f, 5f)]
        private float duration = 1f;

        [BoxGroup("Property"), SerializeField] private bool isLoop = false;

        [BoxGroup("Property"), ShowIf("isLoop"), SerializeField]
        private LoopType loopType;

        [BoxGroup("Property"), SerializeField] private Vector2[] scaleSequence =
        {
            new Vector2(0f, 0f),
            new Vector2(1.2f, 0.8f),
            new Vector2(0.9f, 1.1f),
            new Vector2(1f, 1f)
        };

        private Sequence sequence;
        public event Action OnShow;

        public void Show()
        {
            DoPlay();
        }

        public void Rewind() =>
            DoPlay(false);

        public void Hide()
        {
            if (sequence != null)
            {
                sequence.Kill();
                DOTween.Kill(sequence);
            }
        }

        private void DoPlay(bool playForward = true)
        {
            Vector2[] tmpScaleSequence = playForward ? scaleSequence : scaleSequence.Reverse().ToArray();

            float time = duration / scaleSequence.Length;

            transform.localScale = tmpScaleSequence[0];

            // Killed together with the object on a scene reload
            sequence = DOTween.Sequence().SetLink(gameObject);
            sequence.SetEase(Ease.Flash);

            if (isLoop)
                sequence.SetLoops(int.MaxValue, LoopType.Restart);

            sequence.AppendInterval(delay);

            foreach (Vector2 scale in scaleSequence)
                sequence.Append(transform.DOScale(new Vector3(scale.x, scale.y, 1), time));

            sequence.OnComplete(() => OnShow?.Invoke());
        }
    }
}