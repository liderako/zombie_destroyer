using UnityEngine;

namespace Gameplay.Stack
{
    public class Tray : MonoBehaviour
    {
        [SerializeField] private ItemStack itemStack;
        [SerializeField] private new Renderer renderer;

        private bool canShown = true;

        private void Awake()
        {
            OnStackChanged();
            itemStack.OnVisualChanged += SetCanShown;
            itemStack.OnChanged += OnStackChanged;
        }

        private void SetCanShown(bool obj)
        {
            if (canShown == obj)
                return;

            canShown = obj;
            OnStackChanged();
        }

        private void OnStackChanged()
        {
            renderer.enabled = canShown && enabled && itemStack.Count > 0;
        }

        private void OnEnable()
        {
            OnStackChanged();
        }

        private void OnDisable()
        {
            OnStackChanged();
        }
    }
}