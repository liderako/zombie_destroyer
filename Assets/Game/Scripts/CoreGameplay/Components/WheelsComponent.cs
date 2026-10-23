using Gameplay.Components.Player.Interfaces;
using UnityEngine;

namespace Game.Scripts.CoreGameplay.Components
{
    public class WheelsComponent : MonoBehaviour
    {
        [SerializeField] private BaseMovementComponent movementComponent;
        [SerializeField] private Transform[] rotateComponent;
        [SerializeField] private float speed;

        private bool isInteractable;

        private void OnEnable()
        {
            movementComponent.UpdateSpeed += OnHandleUpdateSpeed;
        }

        private void OnDisable()
        {
            movementComponent.UpdateSpeed -= OnHandleUpdateSpeed;
        }

        private void OnHandleUpdateSpeed(float speed)
        {
            if (speed > 0 && !isInteractable)
            {
                isInteractable = true;
            }
            else if (speed == 0 && isInteractable)
            {
                isInteractable = false;
            }
        }

        private void Update()
        {
            if (isInteractable)
            {
                foreach (var v in rotateComponent)
                {
                    v.Rotate(Vector3.right, speed * Time.deltaTime);
                }
            }
        }
    }
}