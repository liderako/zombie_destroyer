using UnityEngine;
using UnityEngine.UI;

namespace GeneralUI.Components
{
    public abstract class BaseCanvasUI : MonoBehaviour
    {
        public uint windowID { get; protected set; }
    
        protected Canvas canvas;
        protected CanvasScaler canvasScaler;

        private IViewUI[] views;

        public virtual void Init(Canvas canvas)
        {
            this.canvas = canvas;
            canvasScaler = this.canvas.GetComponent<CanvasScaler>();

            views = GetComponentsInChildren<IViewUI>(true);
        }

        public virtual void SetActive(bool active)
        {
            gameObject.SetActive(active);
        }

        public virtual void Show()
        {
            gameObject.SetActive(true);
        }

        public virtual void Hide()
        {
            gameObject.SetActive(false);
        }

        protected virtual void OnEnable()
        {
            if(canvas == null)
                return;

            views.Show();
        }

        protected virtual void OnDisable()
        {
            if(canvas == null)
                return;
        
            views.Hide();
        }

        public virtual bool ActiveSelf() => gameObject.activeSelf;

        public virtual Vector2 ScreenPointToUIPoint(Vector2 point)
        {
            return point / canvas.scaleFactor;
        }

        public virtual void Subscribe() {}
    }
}