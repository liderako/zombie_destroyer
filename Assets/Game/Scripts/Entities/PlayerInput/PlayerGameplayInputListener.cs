using Entities.Turret;
using Game.Scripts.CoreGameplay.Components;
using UnityEngine;
using UnityEngine.EventSystems;
using VContainer;

namespace Entities.PlayerInput
{
    public class PlayerGameplayInputListener : MonoBehaviour
    {
        private const float AIM_DISTANCE = 200f;

        [Inject] private ProgressTrackerComponent progressTracker;
        [Inject] private Camera mainCamera;

        [SerializeField] private TurretAimComponent turretAim;
        [Tooltip("What the finger can point at: zombies and the ground")]
        [SerializeField] private LayerMask aimMask;

        private bool isStarted;
        private bool isAiming;
        private bool isInteractable = true;

        private void OnEnable()
        {
            progressTracker.OnFinish += Disable;
            progressTracker.OnFailGame += Disable;
        }

        private void OnDisable()
        {
            progressTracker.OnFinish -= Disable;
            progressTracker.OnFailGame -= Disable;
        }

        private void Update()
        {
            if (!isInteractable)
            {
                return;
            }

            if (Input.GetMouseButtonDown(0))
            {
                OnPress();
            }

            if (isAiming && Input.GetMouseButton(0))
            {
                turretAim.AimAt(GetPointerWorldPoint());
            }
            else
            {
                isAiming = false;
            }
        }

        private void OnPress()
        {
            if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
            {
                return;
            }

            if (!isStarted)
            {
                isStarted = true;
                progressTracker.CallStartGameEvent();
            }

            isAiming = true;
        }

        // A zombie or the ground under the finger; above the horizon — a far point along the same ray
        private Vector3 GetPointerWorldPoint()
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, AIM_DISTANCE, aimMask, QueryTriggerInteraction.Ignore))
            {
                return hit.point;
            }

            return ray.GetPoint(AIM_DISTANCE);
        }

        private void Disable()
        {
            isInteractable = false;
            isAiming = false;
        }
    }
}
