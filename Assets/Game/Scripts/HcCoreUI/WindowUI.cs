using System.Linq;
using UnityEngine;

namespace GeneralUI.Components
{
    public class WindowUI : MonoBehaviour
    {
        private IViewUI[] views;

        private void Awake()
        {
            views = GetComponentsInChildren<IViewUI>(true);
        }

        protected virtual void OnEnable()
        {
            if (!CheckViewsLength())
                return;

            views.Show();
        }

        protected virtual void OnDisable()
        {
            if (!CheckViewsLength())
                return;

            views.Hide();
        }

        public void Rewind()
        {
            if (!CheckViewsLength())
                return;

            views.Rewind();
        }

        public float GetMaxDuration()
        {
            if (!CheckViewsLength())
                return 0;

            return views.Where(view => !view.IgnoreDuration).Max(view => view.Duration);
        }

        private bool CheckViewsLength()
        {
            if (views.Length == 0)
            {
                Debug.LogAssertion($"IViewUI not found");
                return false;
            }

            return true;
        }
    }
}