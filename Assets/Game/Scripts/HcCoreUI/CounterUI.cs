using DG.Tweening;
using TMPro;
using UnityEngine;

namespace GeneralUI.Components
{
    public class CounterUI : MonoBehaviour
    {
        public RectTransform Icon => icon;

        [SerializeField] private RectTransform icon;
        [SerializeField] private TMP_Text label;

        public void Set(int amount)
        {
            label.text = $"{amount}";
        }

        public void UnscaleUI()
        {
            transform.localScale = Vector3.zero;
        }

        public void StartScaleAnimation()
        {
            transform.DOScale(1f, 0.45f).SetEase(Ease.OutBack).SetLink(gameObject);
        }

        public void LerpPlay(int from, int to)
        {
            float t = 0f;
            DOTween.To(() => t, x => t = x, 1f, 1.5f)
                .SetLink(gameObject)
                .OnUpdate(() => { label.text = $"{(int)Mathf.Lerp(from, to, t)}"; });
        }
    }
}
