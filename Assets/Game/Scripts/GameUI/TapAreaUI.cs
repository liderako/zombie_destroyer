using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace GameUI
{
    // Unlocks after a delay, so the finger that was aiming when the round ended doesn't skip the screen
    public class TapAreaUI : MonoBehaviour
    {
        [SerializeField] private Button button;
        [SerializeField] private float unlockDelay = 0.8f;

        private void OnEnable()
        {
            button.interactable = false;
            DOVirtual.DelayedCall(unlockDelay, () => button.interactable = true).SetLink(gameObject);
        }
    }
}
