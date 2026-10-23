using Gameplay.Components.Player.Interfaces;
using UnityEngine;
using Utils;

namespace Game.Scripts.CoreGameplay.Components
{
    public class VfxTrailListener : MonoBehaviour
    {
        [SerializeField] private ParticleSystem[] particleSystemsMovement;
        [SerializeField] private Ref<ISpeedSource> speedSource;
        [SerializeField] private float Timer = 0.1f;

        private float lastTiming;
        
        private void OnEnable()
        {
            speedSource.Value.UpdateSpeed += OnHandleUpdateSpeed;
        }

        private void OnDisable()
        {
            speedSource.Value.UpdateSpeed -= OnHandleUpdateSpeed;
        }

        private void OnHandleUpdateSpeed(float speed)
        {
            if (speed <= 0)
            {
                return;
            }
            if (Time.unscaledTime - lastTiming >= Timer)
            {
                for (int i = 0; i < particleSystemsMovement.Length; i++)
                {
                    particleSystemsMovement[i].Emit(1);
                }

                lastTiming = Time.unscaledTime;
            }
        }
    }
}