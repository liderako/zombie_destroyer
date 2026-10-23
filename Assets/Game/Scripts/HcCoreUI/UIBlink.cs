using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.Serialization;

namespace GeneralUI.Components
{
    public class UIBlink : MonoBehaviour
    {
        [SerializeField] private RectTransform target;

        [Space] [SerializeField, Min(0.1f), Range(0.1f, 10f)]
        private float delay = 0.1f;

        [SerializeField, Min(0.1f), Range(0.1f, 10f)]
        private float duration = 0.1f;

        [Space] [SerializeField] private Vector2 start;

        [SerializeField] private Vector2 finish;

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

            target.anchoredPosition = start;

            sequence.AppendInterval(delay);
            sequence.Append(target.DOLocalMove(finish, duration));
            sequence.OnComplete(() => { target.anchoredPosition = start; });

            sequence.SetEase(Ease.InBounce);
            sequence.SetLoops(int.MaxValue, LoopType.Restart);

        }

        private void Stop()
        {
            // DOTween.Kill(sequence) kills tweens by target / id, not the sequence itself
            sequence?.Kill();
        }
    }
}