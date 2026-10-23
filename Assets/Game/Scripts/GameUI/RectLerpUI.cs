using DG.Tweening;
using GeneralUI.Components;
using Sirenix.OdinInspector;
using UnityEngine;

namespace GameUI
{
    public class RectLerpUI : MonoBehaviour, IViewUI
    {
        public float Delay => delay;
        public float Duration => duration;
        public bool IgnoreDuration => ignoreDuration;

        [BoxGroup("General"), SerializeField] private float delay;
        [BoxGroup("General"), SerializeField] private bool ignoreDuration;
    
        [BoxGroup("Property"), SerializeField] private Ease easeCurve = Ease.Linear;
    
        [BoxGroup("Property"), SerializeField] private float duration = 1f;
    
        [BoxGroup("Property"), SerializeField] private Vector2 start;
    
        [BoxGroup("Property"), SerializeField] private Vector2 finish;
    
        private RectTransform rectTransform;
    
        private Sequence sequence;
    
        public void Show()
        {
            Play(start, finish);
        }

        public void Rewind()
        {
            Play(finish, start);
        }

        public void Hide()
        {
            if(sequence != null)
            {
                sequence.Kill();
                DOTween.Kill(sequence);
            }
        }

        private void Play(Vector2 from, Vector2 to)
        {
            if(!rectTransform)
                rectTransform = GetComponent<RectTransform>();
        
            rectTransform.sizeDelta = from;
        
            // Killed together with the object on a scene reload
            sequence = DOTween.Sequence().SetLink(gameObject);
            sequence.SetEase(easeCurve);
            sequence.AppendInterval(delay);
            sequence.Append(rectTransform.DOSizeDelta(to, duration));
        }
    }
}