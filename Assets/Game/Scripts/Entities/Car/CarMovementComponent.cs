using System;
using Game.Scripts.CoreGameplay.Components;
using Gameplay.Components.Player.Interfaces;
using HcCoreConfigs.Settings;
using UnityEngine;
using VContainer;

namespace Entities.Car
{
    public class CarMovementComponent : MonoBehaviour, ISpeedSource
    {
        private const float HEADING_SAMPLE_DISTANCE = 0.5f;

        [Inject] private ProgressTrackerComponent progressTracker;

        [SerializeField] private CarSettings settings;

        private bool isDriving;
        private bool isFinished;
        private float distance;
        private float finishDistance;
        private int reportedMeters;
        private Vector3 startPosition;
        private Vector3 roadForward;
        private Vector3 roadRight;

        public event Action<float> UpdateSpeed;

        public float Speed { get; private set; }
        // Along the road, without the weave: zombies lead to where the car will be
        public Vector3 Velocity => roadForward * Speed;

        private void Awake()
        {
            startPosition = transform.position;
            roadForward = transform.forward;
            roadRight = transform.right;
        }

        private void OnEnable()
        {
            progressTracker.OnStartGame += OnHandleStartGame;
            progressTracker.OnFinish += OnHandleFinish;
            progressTracker.OnFailGame += OnHandleFailGame;
        }

        private void OnDisable()
        {
            progressTracker.OnStartGame -= OnHandleStartGame;
            progressTracker.OnFinish -= OnHandleFinish;
            progressTracker.OnFailGame -= OnHandleFailGame;
        }

        private void Update()
        {
            // Past the finish the car keeps full speed for the run-out, then brakes
            bool isRunningOut = !isFinished || distance < finishDistance + settings.finishRunOut;
            float targetSpeed = isDriving && isRunningOut ? settings.maxSpeed : 0f;
            float rate = targetSpeed > Speed ? settings.acceleration : settings.deceleration;
            Speed = Mathf.MoveTowards(Speed, targetSpeed, rate * Time.deltaTime);

            distance += Speed * Time.deltaTime;
            MoveAlongRoad();

            UpdateSpeed?.Invoke(Speed);
            ReportProgress();
        }

        private void MoveAlongRoad()
        {
            float offset = GetWeaveOffset(distance);
            // The heading follows the weave slope, so the car steers instead of sliding sideways
            float slope = (GetWeaveOffset(distance + HEADING_SAMPLE_DISTANCE) - offset) / HEADING_SAMPLE_DISTANCE;
            transform.SetPositionAndRotation(
                startPosition + roadForward * distance + roadRight * offset,
                Quaternion.LookRotation(roadForward + roadRight * slope));
        }

        // Sine weave along the road, faded in over the first half wave so the car starts straight
        private float GetWeaveOffset(float roadDistance)
        {
            float fadeIn = Mathf.SmoothStep(0f, 1f, roadDistance / (settings.weaveWavelength * 0.5f));
            return Mathf.Sin(roadDistance / settings.weaveWavelength * 2f * Mathf.PI) * settings.weaveAmplitude * fadeIn;
        }

        private void ReportProgress()
        {
            // Capped at the finish: reaching it raises OnFinish once
            int meters = Mathf.Min(Mathf.FloorToInt(distance), Mathf.FloorToInt(finishDistance));
            if (!isDriving || isFinished || meters <= reportedMeters)
            {
                return;
            }

            int delta = meters - reportedMeters;
            reportedMeters = meters;
            progressTracker.UpdateProgress(delta);
        }

        private void OnHandleStartGame()
        {
            finishDistance = progressTracker.MaxProgressValue;
            isDriving = true;
        }

        private void OnHandleFinish()
        {
            isFinished = true;
        }

        private void OnHandleFailGame()
        {
            isDriving = false;
        }
    }
}
