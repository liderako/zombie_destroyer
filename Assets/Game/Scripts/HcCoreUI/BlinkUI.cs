using UnityEngine;
using DG.Tweening;
using Sirenix.OdinInspector;

namespace GeneralUI.Components
{
    public class BlinkUI : MonoBehaviour
    {
        [BoxGroup("General"), SerializeField] private RectTransform target;

        [BoxGroup("Property"), SerializeField, Min(0.1f), Range(0.1f, 10f)]
        private float delay = 0.1f;

        [BoxGroup("Property"), SerializeField, Min(0.1f), Range(0.1f, 10f)]
        private float duration = 0.1f;

        [BoxGroup("Property"), SerializeField] private Ease easeCurve = Ease.Linear;

        [BoxGroup("Property"), SerializeField] private Vector2 start;

        [BoxGroup("Property"), SerializeField] private Vector2 finish;

        private Sequence sequence;

        private void OnEnable()
        {
            Stop();
            Play();
        }

        private void OnDisable()
        {
            Stop();
        }

        private void Play()
        {
            sequence = DOTween.Sequence();
            sequence.SetEase(easeCurve);
            sequence.SetLoops(int.MaxValue, LoopType.Restart);

            target.anchoredPosition = start;

            sequence.AppendInterval(delay);
            sequence.Append(target.DOLocalMove(finish, duration));
            sequence.OnComplete(() => { target.anchoredPosition = start; });
        }

        private void Stop()
        {
            if (sequence != null)
            {
                sequence.Kill();
                DOTween.Kill(sequence);
            }
        }
    }
}