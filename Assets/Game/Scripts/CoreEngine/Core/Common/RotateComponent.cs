using UnityEngine;

namespace Common
{
    public enum Axis
    {
        Forward = 0,
        Left = 1,
        Right = 2,
        Up = 3,
        Down = 4
    }

    public class RotateComponent : MonoBehaviour
    {
        public float speed;
        public Axis axis;
        private Vector3 direction;

        private void Awake()
        {
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
            transform.Rotate(direction, speed * Time.deltaTime);
        }
    }
}