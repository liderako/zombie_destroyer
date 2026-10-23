using UnityEngine;

namespace Gameplay.Stack.Item.Utils
{
    public abstract class ItemMover : MonoBehaviour
    {
        protected const float DistThreshold = 0.05f;
        protected const float DistThresholdSqr = DistThreshold * DistThreshold;
        private const float MoveDuration = 0.75f;
        private const float MinSpeed = 1.5f;

        protected Vector3 Offset;
        protected abstract Vector3 CurrentPosition { get; set; }
        protected abstract Vector3 TargetPosition { get; }

        public void AddOffset(in Vector3 value)
        {
            SetOffset(Offset + value);
        }

        public abstract void SetOffset(in Vector3 value);
        public void AddOffsetSilently(in Vector3 value)
        {
            Offset += value;
        }

        public void SetOffsetSilently(in Vector3 value)
        {
            Offset = value;
        }

        public void Warp() { }

        protected abstract void Move(float deltaTime);

        protected abstract void Reached();


        protected static void GetSpeed(float distance, out float duration, out float speed)
        {
            duration = distance == 0
                ? 0.1f
                : MoveDuration;

            speed = distance / duration;
            if (speed < MinSpeed)
            {
                speed = MinSpeed;
                duration = distance / speed;
            }
        }
    }
}