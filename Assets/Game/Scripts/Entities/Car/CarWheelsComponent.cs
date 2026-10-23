using UnityEngine;

namespace Entities.Car
{
    public class CarWheelsComponent : MonoBehaviour
    {
        [SerializeField] private CarMovementComponent movement;
        [SerializeField] private Transform[] wheels;
        [SerializeField] private float wheelRadius = 0.36f;

        private void Update()
        {
            if (movement.Speed <= 0f)
            {
                return;
            }

            float angle = movement.Speed * Time.deltaTime / wheelRadius * Mathf.Rad2Deg;
            Vector3 axis = movement.transform.right;
            foreach (Transform wheel in wheels)
            {
                wheel.Rotate(axis, angle, Space.World);
            }
        }
    }
}
