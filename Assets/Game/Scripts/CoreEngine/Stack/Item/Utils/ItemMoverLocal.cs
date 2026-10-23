using Unity.Mathematics;
using UnityEngine;

namespace Gameplay.Stack.Item.Utils
{
    public sealed class ItemMoverLocal : ItemMover
    {
        private float speed;

        protected override Vector3 CurrentPosition
        {
            get => transform.localPosition;
            set => transform.localPosition = value;
        }

        protected override Vector3 TargetPosition => Offset;

        public override void SetOffset(in Vector3 value)
        {
            Offset = value;
            float distSqr = (CurrentPosition - value).sqrMagnitude;

            if (distSqr <= DistThreshold)
            {
                Reached();
            }
            else
            {
                GetSpeed(math.sqrt(distSqr), out _, out speed);
                enabled = true;
            }
        }

        protected override void Move(float deltaTime)
        {
            Vector3 currentPos = CurrentPosition;
            currentPos = Vector3.MoveTowards(currentPos, Offset, speed * deltaTime);
            float distLeftSqr = (currentPos - Offset).sqrMagnitude;

            if (distLeftSqr > DistThresholdSqr)
            {
                CurrentPosition = currentPos;
            }
            else
            {
                Reached();
                enabled = false;
            }
        }

        private void Update()
        {
            Move(Time.deltaTime);
        }


        protected override void Reached()
        {
            transform.localPosition = Offset;
        }
    }
}