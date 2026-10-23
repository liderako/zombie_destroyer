using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace GeneralUI.Components
{
    public class ClickScale : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        [SerializeField] private Selectable selectable = null;
        [SerializeField] private Ease ease = Ease.OutBack;
        
        [SerializeField] private float scale = 0.95f;
        [SerializeField] private float timeUp = 0.1f;
        [SerializeField] private float timeDown = 0.1f;

        private Tween tweenScale;
        
        private void Start()
        {
            if (!selectable)
                selectable = GetComponent<Selectable>();
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (selectable && selectable.interactable)
            {
                tweenScale?.Complete();
                tweenScale = transform.DOScale(scale, timeDown).SetEase(ease).SetUpdate(true);
            }
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (selectable && selectable.interactable)
            {
                tweenScale?.Complete();
                tweenScale = transform.DOScale(1.0f, timeUp).SetEase(ease).SetUpdate(true);
            }
        }

        private void OnDestroy() => 
            tweenScale?.Kill();

    }
}