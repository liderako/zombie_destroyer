
using Common.Interfaces;
using DG.Tweening;
using UnityEngine;

namespace Common
{
    public class TweenRotateComponent : MonoBehaviour, IEnabledComponent
    {
        [SerializeField] private bool simpleRotate = true;
        public float speed;
        public Axis axis;
        
        [Header("Complex Rotate Logic")]
        public float accelerationTime = 0.4f;
        public float decelerationTime = 0.1f;
        public float minSpeed = 80;
        public Ease Ease = Ease.InOutSine;
        
        private Vector3 direction;
        private float currentSpeed;
        private Tween tweenAcceleration;

        private void Awake()
        {
            if (simpleRotate)
            {
                currentSpeed = speed;
            }
            switch (axis)
            {
                case Axis.Forward:
                    direction = Vector3.forward;
                    break;
                case Axis.Left:
                    direction = Vector3.left;
                    break;
                case Axis.Right:
                    direction = Vector3.right;
                    break;
                case Axis.Up:
                    direction = Vector3.up;
                    break;
                case Axis.Down:
                    direction = Vector3.down;
                    break;
            }
        }

        private void Update()
        {
            transform.Rotate(direction, currentSpeed * Time.deltaTime);
        }

        public void Enable()
        {
            enabled = true;
            tweenAcceleration?.Kill();
            tweenAcceleration = DOTween.To(() => currentSpeed, x => currentSpeed = x, speed, accelerationTime).SetEase(Ease);
        }

        public void Disable()
        {
            tweenAcceleration?.Kill();
            tweenAcceleration = DOTween.To(() => currentSpeed, x => currentSpeed = x, minSpeed, decelerationTime).SetEase(Ease).OnComplete((
                () =>
                {
                    enabled = false;
                }));
        }
    }
}