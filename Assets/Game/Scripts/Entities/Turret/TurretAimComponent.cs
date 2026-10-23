using HcCoreConfigs.Settings;
using UnityEngine;

namespace Entities.Turret
{
    public class TurretAimComponent : MonoBehaviour
    {
        private const float MIN_AIM_DISTANCE = 0.5f;

        [SerializeField] private TurretSettings settings;
        [SerializeField] private Transform pivot;

        private Quaternion baseRotation;
        private float targetWorldYaw;
        private float currentYaw;
        private float yawVelocity;

        private void Awake()
        {
            baseRotation = pivot.localRotation;
            targetWorldYaw = pivot.parent.eulerAngles.y;
        }

        private void Update()
        {
            // The aim is held in world space, so the turret keeps its direction while the car weaves
            float targetYaw = Mathf.DeltaAngle(pivot.parent.eulerAngles.y, targetWorldYaw);
            currentYaw = Mathf.SmoothDampAngle(currentYaw, targetYaw, ref yawVelocity, settings.aimSmoothTime);
            pivot.localRotation = baseRotation * Quaternion.Euler(0f, currentYaw, 0f);
        }

        // Turns the turret towards a world point (the point under the finger); the height of the point is ignored
        public void AimAt(Vector3 worldPoint)
        {
            Vector3 direction = worldPoint - pivot.position;
            direction.y = 0f;
            if (direction.sqrMagnitude < MIN_AIM_DISTANCE * MIN_AIM_DISTANCE)
            {
                return;
            }

            targetWorldYaw = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
        }
    }
}
