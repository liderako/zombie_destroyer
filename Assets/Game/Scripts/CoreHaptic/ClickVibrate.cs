using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using VContainer;

namespace Haptic.Components
{
    public class ClickVibrate : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        [Serializable]
        private enum ActionType
        {
            OnPointerDown,
            OnPointerUp,
        }
        
        [Inject] private HapticComponent hapticComponent;
        [SerializeField] private Selectable selectable;
        [SerializeField] private ActionType actionType = ActionType.OnPointerDown;

        public void OnPointerDown(PointerEventData eventData)
        {
            Vibrate(ActionType.OnPointerDown);   
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            Vibrate(ActionType.OnPointerUp);   
        }

        private void Vibrate(ActionType actionType)
        {
            if (this.actionType != actionType || !selectable || !selectable.interactable)
                return;
            hapticComponent.LightImpact();
        }
    }
}