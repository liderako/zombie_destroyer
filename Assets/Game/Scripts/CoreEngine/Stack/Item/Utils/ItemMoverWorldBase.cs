using System;
using Sirenix.OdinInspector;
using Unity.Mathematics;
using UnityEngine;

namespace Gameplay.Stack.Item.Utils
{
    public abstract class ItemMoverWorldBase : ItemMover
    {
        protected virtual float YJump => 1;

        public System.Action OnReached;

        [System.NonSerialized] public float SpeedMultiplier = 1;

        /// <summary> [0,1] depends on distance to target, doesn't decrease  </summary>
        private float t;

        /// <summary> [0...] depends on time and distance, uses highest </summary>
        private float tConstant;

        /// <summary> Updates until middle point reached </summary>
        private float yMax;

        private Vector3 currentPos;


        private float initialDist;
        private Vector3 initialPos;
        private Quaternion initialRot;

        private float speed;
        [ReadOnly, ShowInInspector] private float duration;

        protected override Vector3 CurrentPosition
        {
            get => transform.position;
            set => transform.position = value;
        }

        protected abstract override Vector3 TargetPosition { get; }
        protected abstract Quaternion TargetRotation { get; }
        protected abstract Vector3 TargetRotationEuler { get; }

        [ShowInInspector] private float CurrentSpeed => speed * SpeedMultiplier;

        // private float TMiddleMax => Mathf.Sin(Mathf.PI * t);


        private void OnEnable()
        {
            initialPos = transform.position;
            initialRot = transform.rotation;
            t = 0;
            tConstant = 0;
            yMax = float.MinValue;
        }

        private void OnDisable()
        {
            Offset = Vector3.zero;
        }

        public override void SetOffset(in Vector3 value)
        {
            Offset = value;
            currentPos = transform.position;
            Vector3 targetPosition = TargetPosition;
            float distSqr = (currentPos - targetPosition).sqrMagnitude;

            if (distSqr <= DistThreshold)
            {
                Reached();
            }
            else
            {
                if (enabled)
                {
                    float newYMax = YJump + math.max(initialPos.y, targetPosition.y);
                    if (t <= 0.5f)
                    {
                        yMax = math.max(currentPos.y, newYMax);
                        t = math.unlerp(initialPos.y, yMax, currentPos.y) / 2;
                    }
                    else
                    {
                        if (currentPos.y > newYMax)
                            yMax = currentPos.y;
                        t = 0.5f + math.min(0.49f, math.unlerp(yMax, targetPosition.y, currentPos.y) / 2);
                    }

                    // tConstant = 0;
                    // for (int i = 0; i < 4; i++)
                    // {
                    //     tConstant += Mathf.InverseLerp(initialRot[i], targetRot[i], currentRot[i]) / 4;
                    // }
                    float newDist = math.sqrt(distSqr);

                    initialDist = newDist / (1 - t);
                    tConstant = t;
                    initialRot = Quaternion.Euler(
                        (transform.eulerAngles - t * TargetRotationEuler)
                        / (1 - t));
                }
                else
                {
                    transform.parent = null;
                    initialDist = math.sqrt(distSqr);
                    enabled = true;
                }

                GetSpeed(initialDist, out duration, out speed);
            }
        }

        protected override void Move(float deltaTime)
        {
            float speedMultiplier = SpeedMultiplier * (1 + tConstant) * deltaTime;

            Vector3 targetPosition = TargetPosition;
            currentPos = Vector3.MoveTowards(currentPos, targetPosition, speed * speedMultiplier);
            float distLeft = (currentPos - targetPosition).magnitude;
            if (distLeft > DistThreshold)
            {
                t = math.max(t, 1 - math.clamp((distLeft - DistThreshold) / initialDist, 0f, 1f));
                tConstant = math.max(t, tConstant + speedMultiplier / duration);

                Vector3 newPos = currentPos;
                {
                    if (t <= 0.5f)
                    {
                        yMax = math.max(yMax, YJump + Mathf.Max(initialPos.y, targetPosition.y));
                        newPos.y = math.lerp(initialPos.y, yMax, t * 2);
                    }
                    else
                    {
                        newPos.y = math.lerp(yMax, targetPosition.y, (t - 0.5f) * 2);
                    }
                }

                transform.rotation = Quaternion.Lerp(initialRot, TargetRotation, tConstant);
                transform.position = newPos;
            }
            else
            {
                Reached();
                enabled = false;
            }
        }


        private void FixedUpdate()
        {
            Move(Time.fixedDeltaTime);
        }


        protected override void Reached()
        {
            transform.localPosition = Offset;
            transform.localRotation = Quaternion.identity;
            OnReached?.Invoke();
        }
    }
}