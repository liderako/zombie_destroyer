using Unity.Cinemachine;
using UnityEngine;

namespace Game.Scripts.CoreGameplay.Components
{
    [RequireComponent(typeof(CinemachineCamera))]
    public class CinemachineShakeComponent : MonoBehaviour
    {
        private CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin;
        private float shakeTimer;

        private void Awake()
        {
            cinemachineBasicMultiChannelPerlin = GetComponent<CinemachineCamera>().GetCinemachineComponent(CinemachineCore.Stage.Noise) as  CinemachineBasicMultiChannelPerlin;
        }

        public void ShakeCamera(float intensity, float time)
        {
            cinemachineBasicMultiChannelPerlin.AmplitudeGain = intensity;
            shakeTimer = time;
        }

        private void Update()
        {
            if (shakeTimer > 0)
            {
                shakeTimer -= Time.deltaTime;
                if (shakeTimer <= 0f)
                {
                    cinemachineBasicMultiChannelPerlin.AmplitudeGain = 0f;
                }
            }
        }
    }
}