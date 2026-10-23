using System;
using UnityEngine;
using VContainer;

namespace Gameplay.Components.Player.Interfaces
{
    public abstract class BaseMovementComponent : MonoBehaviour, ISpeedSource
    {
        [Inject] protected IVariableJoystick Joystick;
        public Rigidbody Rigidbody { get; set; }
        public Quaternion LastRotation { get; set; }
        public float SpeedModificator { get; set; }

        public bool BlockerMovement { get; set; }

        public virtual event Action<float> UpdateSpeed;
        
        public Vector3 OriginPosition {get; protected set;}
        public Quaternion OriginRotation {get; protected set;}

        protected float Speed;
        protected virtual void Awake()
        {
            SpeedModificator = 0;
            LastRotation = transform.rotation;
            Rigidbody = GetComponent<Rigidbody>();
            SaveOriginTransform();
        }

        private void OnHandleBlockMovementAction(bool state)
        {
            BlockerMovement = state;
            Rigidbody.linearVelocity = Vector3.zero;
            SpeedModificator = 0;
            UpdateSpeed?.Invoke(0);
        }

        private void SaveOriginTransform()
        {
            OriginPosition = transform.position;
            OriginRotation = transform.rotation;
        }
        
        public void ResetToOriginPosition()
        {
            transform.position = OriginPosition;
            transform.rotation = OriginRotation;
        }

        public virtual void Run()
        {
            if (BlockerMovement)
            {
                return;
            }
            float speedTarget = GetSpeed();
            Speed = Mathf.Lerp(Speed, speedTarget, 0.15f);
            Vector3 movement = new Vector3(Joystick.Horizontal, 0, Joystick.Vertical);
            movement.Normalize();

            if (movement != Vector3.zero)
            {
                transform.position += movement * Speed * Time.deltaTime;
                transform.rotation = Quaternion.LookRotation(movement, transform.up);
                LastRotation = transform.rotation;

                float totalSpeed = Speed;
                UpdateSpeed?.Invoke(totalSpeed);
            }
            else
            {
                transform.rotation = LastRotation;
                UpdateSpeed?.Invoke(0);
            }
        }

        public virtual void FixedUpdate()
        {
            Run();
            SpeedModificator = Mathf.Lerp(SpeedModificator, 0, 0.05f);
            if (SpeedModificator < 0.01f)
            {
                SpeedModificator = 0;
            }
            Rigidbody.linearVelocity = Vector3.Lerp(Rigidbody.linearVelocity, Vector3.zero, 0.5f);
        }

        public abstract float GetSpeed();
    }
}